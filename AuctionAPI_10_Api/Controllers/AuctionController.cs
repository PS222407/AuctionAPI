using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuctionController(
    IAuctionService auctionService,
    IConfiguration configuration,
    IProductService productService,
    IValidator<AuctionRequest> validator)
    : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(auctionService.Get().Select(a => new AuctionViewModel
        {
            Id = a.Id,
            Product = new ProductViewModel
            {
                Id = a.Product.Id,
                Name = a.Product.Name,
                Description = a.Product.Description,
                ImageUrl = a.Product.ImageIsExternal
                    ? a.Product.ImageUrl
                    : $"{configuration["BackendUrl"]}{a.Product.ImageUrl}",
            },
            DurationInSeconds = a.DurationInSeconds,
            StartDateTime = a.StartDateTime,
        }));
    }

    [HttpGet("{id:int}")]
    public IActionResult Get([FromRoute] int id)
    {
        Auction? auction = auctionService.GetById(id);
        if (auction == null)
        {
            return NotFound();
        }

        AuctionViewModel auctionViewModel = new()
        {
            Id = auction.Id,
            Product = new ProductViewModel
            {
                Id = auction.Product.Id,
                Name = auction.Product.Name,
                Description = auction.Product.Description,
                ImageUrl = auction.Product.ImageIsExternal
                    ? auction.Product.ImageUrl
                    : $"{configuration["BackendUrl"]}{auction.Product.ImageUrl}",
            },
            DurationInSeconds = auction.DurationInSeconds,
            StartDateTime = auction.StartDateTime,
            Bids = auction.Bids.Select(b => new BidViewModel
            {
                Id = b.Id,
                PriceInCents = b.PriceInCents,
                User = new UserViewModel
                {
                    Id = b.User.Id,
                    Email = b.User.Email,
                },
                CreatedAt = b.CreatedAt,
            }).ToList(),
        };

        return Ok(auctionViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Post([FromBody] AuctionRequest auctionRequest)
    {
        ValidationResult result = validator.Validate(auctionRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        if (!productService.Exists(auctionRequest.ProductId))
        {
            return BadRequest("Product not found");
        }

        Auction auction = new()
        {
            ProductId = auctionRequest.ProductId,
            DurationInSeconds = auctionRequest.DurationInSeconds,
            StartDateTime = auctionRequest.StartDateTime,
        };

        auctionService.Create(auction);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public IActionResult Put([FromRoute] int id, [FromBody] AuctionRequest auctionRequest)
    {
        if (!auctionService.Exists(id))
        {
            return NotFound();
        }

        ValidationResult result = validator.Validate(auctionRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        if (!productService.Exists(auctionRequest.ProductId))
        {
            return BadRequest("Product not found");
        }

        Auction auction = new()
        {
            Id = id,
            ProductId = auctionRequest.ProductId,
            DurationInSeconds = auctionRequest.DurationInSeconds,
            StartDateTime = auctionRequest.StartDateTime,
        };

        auctionService.Update(auction);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!auctionService.Exists(id))
        {
            return NotFound();
        }

        auctionService.Delete(id);

        return NoContent();
    }
}