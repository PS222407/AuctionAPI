using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class AuctionRepository(DataContext dbContext) : IAuctionRepository
{
    public List<Auction> Get()
    {
        return dbContext.Auctions
            .Include(a => a.Product)
            .ToList();
    }

    public Auction? Create(Auction auction)
    {
        dbContext.Auctions.Add(auction);

        if (dbContext.SaveChanges() <= 0)
        {
            return null;
        }

        dbContext.Entry(auction).Reference(a => a.Product).Load();
        return auction;
    }

    public Auction? GetById(long id)
    {
        return dbContext.Auctions
            .Include(a => a.Product)
            .Include(a => a.Bids.OrderByDescending(b => b.PriceInCents))
            .ThenInclude(b => b.User)
            .FirstOrDefault(a => a.Id == id);
    }

    public bool Update(Auction auction)
    {
        dbContext.Auctions.Update(auction);
        return dbContext.SaveChanges() > 0;
    }

    public bool Delete(long id)
    {
        Auction? auction = dbContext.Auctions.FirstOrDefault(a => a.Id == id);
        if (auction == null)
        {
            return false;
        }

        dbContext.Auctions.Remove(auction);
        return dbContext.SaveChanges() > 0;
    }

    public bool Exists(int id)
    {
        return dbContext.Auctions.Any(a => a.Id == id);
    }
}