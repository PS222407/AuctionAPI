using System.Security.Claims;
using AuctionAPI_10_Api.ViewModels;
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

    public UserController(IUserService userService, DataContext context)
    {
        _userService = userService;
        _context = context;
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
    public IActionResult GetAuctions()
    {
        // string? id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        // if (id == null)
        // {
        //     return null;
        // }

        string userId = "c51aee86-3644-4c23-9d7b-113429192561";
        string adminId = "0206A018-5AC6-492D-AB99-10105193D384";

        // SELECT b.UserId, b.AuctionId, b.PriceInCents, a.DurationInSeconds
        // FROM Bids b
        // JOIN Auctions a
        // ON a.Id = b.AuctionId
        // JOIN Products p
        // ON p.Id = a.ProductId
        // JOIN (
        //     SELECT AuctionId, MAX(PriceInCents) AS MaxPrice
        //     FROM Bids
        //     GROUP BY AuctionId
        // ) max_bids ON b.AuctionId = max_bids.AuctionId AND b.PriceInCents = max_bids.MaxPrice;
        
        var result = (
            from b in _context.Bids
            join maxBid in
                from bid in _context.Bids
                group bid by bid.AuctionId
                into g
                select new { AuctionId = g.Key, MaxPrice = g.Max(b => b.PriceInCents) }
                on new { b.AuctionId, b.PriceInCents } equals new { maxBid.AuctionId, PriceInCents = maxBid.MaxPrice }
            select new { b.UserId, b.AuctionId, b.PriceInCents }).ToList();

        return Ok(result);
        // return _userService.GetWonAuctions(userId);
    }
}