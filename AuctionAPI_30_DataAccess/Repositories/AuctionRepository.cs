using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class AuctionRepository : IAuctionRepository
{
    private readonly DataContext _dbContext;

    public AuctionRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Auction> Get()
    {
        return _dbContext.Auctions
            .Include(a => a.Product)
            .ToList();
    }

    public bool Create(Auction auction)
    {
        _dbContext.Auctions.Add(auction);
        return _dbContext.SaveChanges() > 0;
    }

    public Auction? GetById(long id)
    {
        return _dbContext.Auctions
            .Include(a => a.Product)
            .Include(a => a.Bids.OrderByDescending(b => b.PriceInCents))
            .ThenInclude(b => b.User)
            .FirstOrDefault(a => a.Id == id);
    }

    public bool Update(Auction auction)
    {
        _dbContext.Auctions.Update(auction);
        return _dbContext.SaveChanges() > 0;
    }

    public bool Delete(long id)
    {
        Auction? auction = _dbContext.Auctions.FirstOrDefault(a => a.Id == id);
        if (auction == null)
        {
            return false;
        }

        _dbContext.Auctions.Remove(auction);
        return _dbContext.SaveChanges() > 0;
    }

    public bool Exists(int id)
    {
        return _dbContext.Auctions.Any(a => a.Id == id);
    }
}