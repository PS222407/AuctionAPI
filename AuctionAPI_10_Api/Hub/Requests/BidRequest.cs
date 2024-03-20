namespace AuctionAPI_10_Api.Hub.Requests;

public class BidRequest
{
    public UserRequest User { get; set; }

    public int PriceInCents { get; set; }

    public DateTime CreatedAt { get; set; }
}