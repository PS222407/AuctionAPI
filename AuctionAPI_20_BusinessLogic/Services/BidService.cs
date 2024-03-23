using AuctionAPI_20_BusinessLogic.Exceptions;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class BidService(IBidRepository bidRepository, IAuctionService auctionService) : IBidService
{
    public bool Create(Bid bid)
    {
        if (!auctionService.IsRunning(bid.AuctionId))
        {
            throw new AuctionNotAvailableException("Auction is not running");
        }

        Auction auction = auctionService.GetById(bid.AuctionId)!;
        Bid? highestBid = auction.Bids.MaxBy(b => b.PriceInCents);
        if (highestBid != null && highestBid.PriceInCents >= bid.PriceInCents)
        {
            throw new BidTooLowException("Bid is too low");
        }

        return bidRepository.Create(bid);
    }
}