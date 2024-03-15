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
}