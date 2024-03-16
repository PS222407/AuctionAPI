namespace AuctionAPI_10_Api.RequestModels;

public class BidRequest
{
    public long AuctionId { get; set; }
    
    public int PriceInCents { get; set; }
}