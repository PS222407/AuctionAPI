using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;

namespace AuctionAPI_30_DataAccess.Repositories;

public class BidRepository(DataContext dbContext) : IBidRepository
{
    public bool Create(Bid bid)
    {
        dbContext.Bids.Add(bid);
        return dbContext.SaveChanges() > 0;
    }
}