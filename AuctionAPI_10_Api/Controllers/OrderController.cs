using System.Security.Claims;
using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Exceptions;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response;
using Serilog;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController(
    IProductService productService,
    IOrderService orderService,
    IValidator<OrderRequest> validator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Post([FromBody] OrderRequest orderRequest)
    {
        string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        ValidationResult result = await validator.ValidateAsync(orderRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        Product? product = productService.GetById(orderRequest.ProductId);
        if (product == null)
        {
            return NotFound();
        }

        string orderId = Guid.NewGuid().ToString();

        PaymentResponse paymentResponse;
        try
        {
            paymentResponse = await orderService.CreateMolliePayment(product.PriceInCents, orderId);
        }
        catch (MollieApiException e)
        {
            if (e.Details.Status == 401)
            {
                return Unauthorized(new { Message = "Unauthorized Mollie API-Key" });
            }

            return BadRequest(new { Message = "Mollie error" });
        }
        catch (Exception)
        {
            return BadRequest(new { Message = "Payment by Mollie could not be created" });
        }

        Order order;
        try
        {
            order = orderService.Create(product, userId, paymentResponse.Id, orderId);
        }
        catch (DatabaseCreationException e)
        {
            return BadRequest(new { e.Message });
        }

        return CreatedAtAction("Get", new { id = order.Id },
            new OrderCreatedViewModel
            {
                RedirectUrl = paymentResponse.Links.Checkout.Href,
                OrderId = order.Id,
            });
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult Get([FromRoute] string id)
    {
        Order? order = orderService.GetBy(id);
        if (order == null)
        {
            return NotFound(new { Message = "Order not found" });
        }

        return Ok(new OrderViewModel
        {
            Id = order.Id,
            PaymentStatus = order.PaymentStatus.ToString(),
        });
    }

    [DisableCors]
    [HttpPost("webhook")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Webhook([FromForm] WebhookRequest request)
    {
        Log.Information("Webhook received {@request}", request);

        Order? order = orderService.GetByExternalPaymentId(request.id);
        if (order == null)
        {
            return Ok();
        }

        PaymentResponse paymentResponse;
        try
        {
            paymentResponse = await orderService.GetPaymentFromMollie(request.id);
        }
        catch (MollieApiException e)
        {
            return BadRequest(new { e.Message });
        }

        order.PaymentStatus = paymentResponse.Status switch
        {
            PaymentStatus.Paid => AuctionAPI_20_BusinessLogic.Enums.PaymentStatus.Paid,
            PaymentStatus.Canceled => AuctionAPI_20_BusinessLogic.Enums.PaymentStatus.Canceled,
            PaymentStatus.Expired => AuctionAPI_20_BusinessLogic.Enums.PaymentStatus.Expired,
            var _ => AuctionAPI_20_BusinessLogic.Enums.PaymentStatus.Pending,
        };
        if (!orderService.Update(order))
        {
            return BadRequest(new { Message = "Order could not be updated" });
        }

        return Ok();
    }
}