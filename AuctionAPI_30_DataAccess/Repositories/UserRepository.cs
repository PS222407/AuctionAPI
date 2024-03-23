using AuctionAPI_20_BusinessLogic.DataModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    // private readonly DataContext _context;
    //
    // private readonly UserManager<IdentityUser> _userManager;
    //
    // public UserRepository(UserManager<IdentityUser> userManager, DataContext context)
    // {
    //     _userManager = userManager;
    //     _context = context;
    // }
    //
    // public List<IdentityUser> SearchByEmail(string email)
    // {
    //     return _userManager.Users.Where(u => u.Email != null && u.Email.Contains(email)).ToList();
    // }
    //
    // public List<WonAuctionsDataModel> GetWonAuctions(string userId)
    // {
    //     return _context.Database.SqlQuery<WonAuctionsDataModel>($@"
    //         SELECT b.Id b__Id, b.AuctionId b__AuctionId, b.PriceInCents b__PriceInCents, b.CreatedAt b__CreatedAt, b.UserId b__UserId, a.Id a__id, a.ProductId a__ProductId, a.StartDateTime a__StartDateTime, a.DurationInSeconds a__DurationInSeconds, p.Id p__Id, p.Name p__Name, p.Description p__Description, p.ImageUrl p__ImageUrl, p.CategoryId p__CategoryId, p.ImageIsExternal p__ImageIsExternal
    //         FROM Bids b
    //         JOIN Auctions a
    //         ON a.Id = b.AuctionId
    //         JOIN Products p
    //         ON p.Id = a.ProductId
    //         JOIN (
    //            SELECT AuctionId, MAX(PriceInCents) AS MaxPrice
    //            FROM Bids
    //            GROUP BY AuctionId
    //         ) max_bids ON b.AuctionId = max_bids.AuctionId AND b.PriceInCents = max_bids.MaxPrice
    //         WHERE b.UserId = {userId}
    //     ").ToList();
    // }
}