using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IUserRepository
{
    public List<IdentityUser> SearchByEmail(string email);

    public List<Auction> GetWonAuctions(string userId);
}