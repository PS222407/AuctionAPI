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
public class BidController(
    IBidService bidService,
    IHubContext<MainHub, IMainHubClient> hubContext,
    IValidator<BidRequest> validator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BidRequest bidRequest)
    {
        ValidationResult result = await validator.ValidateAsync(bidRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

        bidService.Create(new Bid
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
        await hubContext.Clients.Group($"Auction-{bidRequest.AuctionId}").ReceiveBids(br);

        return NoContent();
    }
}