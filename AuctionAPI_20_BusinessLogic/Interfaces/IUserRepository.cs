using AuctionAPI_20_BusinessLogic.DataModels;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IUserRepository
{
    public List<WonAuctionsDataModel> GetWonAuctions(string userId);
}