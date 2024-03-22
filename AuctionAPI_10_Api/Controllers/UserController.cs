using System.Security.Claims;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IUserService _userService;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet("{email}")]
    public IActionResult Get(string email)
    {
        return Ok(_userService.SearchByEmail(email).Select(x => new UserViewModel
        {
            Id = x.Id,
            Name = x.UserName,
            Email = x.Email,
        }));
    }

    [HttpGet("Info")]
    [Authorize]
    public List<string> GetInfo()
    {
        return User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
    }

    [HttpGet("Auctions/Won")]
    [Authorize]
    public IActionResult GetWonAuctions()
    {
        // string adminId = "0206A018-5AC6-492D-AB99-10105193D384";
        // string employeeId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652";

        string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        return Ok(_userService.GetWonAuctions(userId)
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
                        : $"{_configuration["BackendUrl"]}{x.Product.ImageUrl}",
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