using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;

namespace AuctionAPI_30_DataAccess.Repositories;

public class BidRepository : IBidRepository
{
    private readonly DataContext _dbContext;

    public BidRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool Create(Bid bid)
    {
        _dbContext.Bids.Add(bid);
        return _dbContext.SaveChanges() > 0;
    }
}