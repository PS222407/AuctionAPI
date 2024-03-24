using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IAuctionService
{
    public List<Auction> Get();

    public Auction? Create(Auction auction);

    public Auction? GetById(long id);

    public bool Update(Auction auction);

    public bool Delete(long id);

    public bool IsRunning(long id);

    public bool Exists(int id);
}