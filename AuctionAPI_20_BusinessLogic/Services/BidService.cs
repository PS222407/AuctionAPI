using AuctionAPI_20_BusinessLogic.Exceptions;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class BidService : IBidService
{
    private readonly IBidRepository _bidRepository;

    private readonly IAuctionService _auctionService;

    public BidService(IBidRepository bidRepository, IAuctionService auctionService)
    {
        _bidRepository = bidRepository;
        _auctionService = auctionService;
    }

    public bool Create(Bid bid)
    {
        if (!_auctionService.IsRunning(bid.AuctionId))
        {
            throw new AuctionNotAvailableException("Auction is not running");
        }

        Auction auction = _auctionService.GetById(bid.AuctionId)!;
        Bid? highestBid = auction.Bids.MaxBy(b => b.PriceInCents);
        if (highestBid != null && highestBid.PriceInCents >= bid.PriceInCents)
        {
            throw new BidTooLowException("Bid is too low");
        }

        return _bidRepository.Create(bid);
    }
}