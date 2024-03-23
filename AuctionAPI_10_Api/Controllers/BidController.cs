using System.Security.Claims;
using AuctionAPI_10_Api.Hub;
using AuctionAPI_10_Api.Hub.Requests;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BidRequest = AuctionAPI_10_Api.RequestModels.BidRequest;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BidController : ControllerBase
{
    private readonly IBidService _bidService;

    private readonly IHubContext<MainHub, IMainHubClient> _hubContext;

    private readonly IValidator<BidRequest> _validator;

    public BidController(
        IBidService bidService,
        IHubContext<MainHub, IMainHubClient> hubContext,
        IValidator<BidRequest> validator)
    {
        _bidService = bidService;
        _hubContext = hubContext;
        _validator = validator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BidRequest bidRequest)
    {
        ValidationResult result = await _validator.ValidateAsync(bidRequest);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

        _bidService.Create(new Bid
        {
            AuctionId = bidRequest.AuctionId,
            PriceInCents = bidRequest.PriceInCents,
            CreatedAt = now,
            UserId = userId,
        });

        Hub.Requests.BidRequest br = new()
        {
            User = new UserRequest
            {
                Id = userId,
                Name = User.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                Email = User.Claims.First(c => c.Type == ClaimTypes.Email).Value,
            },
            PriceInCents = bidRequest.PriceInCents,
            CreatedAt = now,
        };
        await _hubContext.Clients.Group($"Auction-{bidRequest.AuctionId}").ReceiveBids(br);

        return NoContent();
    }
}