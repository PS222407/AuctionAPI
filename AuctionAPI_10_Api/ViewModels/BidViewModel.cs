namespace AuctionAPI_10_Api.ViewModels;

public class BidViewModel
{
    public long Id { get; set; }

    public int PriceInCents { get; set; }

    public DateTime CreatedAt { get; set; }

    public UserViewModel User { get; set; }
}