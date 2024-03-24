using System.Security.Claims;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(IUserService userService, IConfiguration configuration) : ControllerBase
{
    [HttpGet("Auctions/Won")]
    [Authorize]
    [ProducesResponseType(200)]
    public IActionResult GetWonAuctions()
    {
        string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        return Ok(userService.GetWonAuctions(userId)
            .Select(x => new AuctionViewModel
            {
                Id = x.Id,
                StartDateTime = x.StartDateTime,
                DurationInSeconds = x.DurationInSeconds,
                Product = new ProductViewModel
                {
                    Id = x.Product.Id,
                    Name = x.Product.Name,
                    Description = x.Product.Description,
                    ImageUrl = x.Product.ImageIsExternal
                        ? x.Product.ImageUrl
                        : $"{configuration["BackendUrl"]}{x.Product.ImageUrl}",
                },
                Bids = x.Bids.Select(b => new BidViewModel
                {
                    Id = b.Id,
                    PriceInCents = b.PriceInCents,
                    CreatedAt = b.CreatedAt,
                }).ToList(),
            }).ToList()
        );
    }
}