using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;

    public AuctionService(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public List<Auction> Get()
    {
        return _auctionRepository.Get();
    }

    public bool Create(Auction auction)
    {
        return _auctionRepository.Create(auction);
    }

    public Auction? GetById(long id)
    {
        return _auctionRepository.GetById(id);
    }

    public bool Update(Auction auction)
    {
        return _auctionRepository.Update(auction);
    }

    public bool Delete(long id)
    {
        return _auctionRepository.Delete(id);
    }

    public bool IsRunning(long id)
    {
        Auction? auction = _auctionRepository.GetById(id);
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
        return _auctionRepository.Exists(id);
    }
}