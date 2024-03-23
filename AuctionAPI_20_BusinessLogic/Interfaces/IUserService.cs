using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IUserService
{
    public List<Auction> GetWonAuctions(string userId);
}