namespace AuctionAPI_10_Api.RequestModels;

public class AuctionRequest
{
    public long ProductId { get; set; }
    
    public DateTime StartDateTime { get; set; }
    
    public int DurationInSeconds { get; set; }
}