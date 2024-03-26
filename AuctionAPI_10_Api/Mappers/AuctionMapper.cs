using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_10_Api.Mappers;

public static class AuctionMapper
{
    public static AuctionViewModel MapToViewModel(Auction auction, IConfiguration configuration)
    {
        if (auction.Product != null)
        {
            auction.Product.Auctions = null;
        }

        if (auction.Bids != null)
        {
            auction.Bids.ForEach(b => b.Auction = null);
        }

        AuctionViewModel auctionViewModel = new()
        {
            Id = auction.Id,
            Product = auction.Product == null ? null : ProductMapper.MapToViewModel(auction.Product, configuration),
            DurationInSeconds = auction.DurationInSeconds,
            StartDateTime = auction.StartDateTime,
            Bids = auction.Bids?.Select(BidMapper.MapToViewModel).ToList(),
        };

        return auctionViewModel;
    }
}