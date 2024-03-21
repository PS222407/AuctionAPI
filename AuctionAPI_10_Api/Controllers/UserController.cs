using System.Security.Claims;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.DataModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    private DataContext _context;

    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, DataContext context, IConfiguration configuration)
    {
        _userService = userService;
        _context = context;
        _configuration = configuration;
    }

    [HttpGet("{email}")]
    public IEnumerable<UserViewModel> Get(string email)
    {
        return _userService.SearchByEmail(email).Select(x => new UserViewModel
        {
            Id = x.Id,
            Name = x.UserName,
            Email = x.Email,
        });
    }

    [HttpGet("Info")]
    [Authorize]
    public List<string>? GetInfo()
    {
        string? id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
        {
            return null;
        }

        return User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
    }

    [HttpGet("Auctions/Won")]
    [Authorize]
    public IActionResult GetWonAuctions()
    {
        // string adminId = "0206A018-5AC6-492D-AB99-10105193D384";
        // string employeeId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652";

        string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        List<AuctionViewModel> auctionViewModels = _userService.GetWonAuctions(userId)
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
            }).ToList();

        return Ok(auctionViewModels);
    }
}