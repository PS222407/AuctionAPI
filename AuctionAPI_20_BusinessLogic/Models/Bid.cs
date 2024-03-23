using Microsoft.AspNetCore.Identity;

namespace AuctionAPI_20_BusinessLogic.Models;

public class Bid
{
    public long Id { get; set; }

    public long AuctionId { get; set; }

    public Auction Auction { get; set; }

    public int PriceInCents { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }
}