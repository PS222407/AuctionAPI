using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly DataContext _context;

    public UserRepository(UserManager<IdentityUser> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public List<IdentityUser> SearchByEmail(string email)
    {
        return _userManager.Users.Where(u => u.Email != null && u.Email.Contains(email)).ToList();
    }

    public List<Auction> GetWonAuctions(string userId)
    {
        var a = _context.Bids
            .GroupBy(b => b.AuctionId)
                .ToList();
        return new List<Auction>();
    }
}