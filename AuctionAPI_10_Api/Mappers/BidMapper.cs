using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_10_Api.Mappers;

public static class BidMapper
{
    public static BidViewModel MapToViewModel(Bid bid)
    {
        return new BidViewModel
        {
            Id = bid.Id,
            PriceInCents = bid.PriceInCents,
            User = UserMapper.MapToViewModel(bid.User),
            CreatedAt = bid.CreatedAt,
        };
    }
}