using System.Security.Claims;
using AuctionAPI_10_Api.Hub;
using AuctionAPI_10_Api.Hub.Requests;
using AuctionAPI_20_BusinessLogic.Exceptions;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
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
    
    public BidController(IBidService bidService, IHubContext<MainHub, IMainHubClient> hubContext)
    {
        _bidService = bidService;
        _hubContext = hubContext;
    }

    [Authorize]
    [HttpPost]
    public IActionResult Post([FromBody] BidRequest bidRequest)
    {
        string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        TimeZoneInfo amsterdamZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, amsterdamZone);
        
        try
        {
            _bidService.Create(new Bid
            {
                AuctionId = bidRequest.AuctionId,
                PriceInCents = bidRequest.PriceInCents,
                CreatedAt = now,
                UserId = userId,
            });
        }
        catch (AuctionNotAvailableException e)
        {
            return NotFound(e.Message);
        }
        catch (BidTooLowException e)
        {
            return BadRequest(e.Message);
        }
        
        Hub.Requests.BidRequest br = new()
        {
            User = new UserRequest
            {
                Id = userId,
                Name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value,
                Email = User.Claims.First(c => c.Type == ClaimTypes.Email).Value,
            },
            PriceInCents = bidRequest.PriceInCents,
            CreatedAt = now,
        };
        _hubContext.Clients.Group($"Auction-{bidRequest.AuctionId}").ReceiveBids(br);
        
        return Ok();
    }
}