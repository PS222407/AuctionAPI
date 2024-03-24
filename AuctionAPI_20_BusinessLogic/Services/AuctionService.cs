using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class AuctionService(IAuctionRepository auctionRepository) : IAuctionService
{
    public List<Auction> Get()
    {
        return auctionRepository.Get();
    }

    public Auction? Create(Auction auction)
    {
        return auctionRepository.Create(auction);
    }

    public Auction? GetById(long id)
    {
        return auctionRepository.GetById(id);
    }

    public bool Update(Auction auction)
    {
        return auctionRepository.Update(auction);
    }

    public bool Delete(long id)
    {
        return auctionRepository.Delete(id);
    }

    public bool IsRunning(long id)
    {
        Auction? auction = auctionRepository.GetById(id);
        if (auction == null)
        {
            return false;
        }

        TimeZoneInfo amsterdamZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, amsterdamZone);
        DateTime endDateTime = auction.StartDateTime.AddSeconds(auction.DurationInSeconds);

        return now > auction.StartDateTime && now < endDateTime;
    }

    public bool Exists(int id)
    {
        return auctionRepository.Exists(id);
    }
}