using System.Security.Claims;
using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_20_BusinessLogic.Exceptions;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BidController : ControllerBase
{
    private readonly IBidService _bidService;
    private readonly IAuctionService _auctionService;

    public BidController(IBidService bidService, IAuctionService auctionService)
    {
        _bidService = bidService;
        _auctionService = auctionService;
    }

    [Authorize]
    [HttpPost]
    public IActionResult Post([FromBody] BidRequest bidRequest)
    {
        string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        try
        {
            TimeZoneInfo amsterdamZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, amsterdamZone);

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

        return Ok();
    }
}