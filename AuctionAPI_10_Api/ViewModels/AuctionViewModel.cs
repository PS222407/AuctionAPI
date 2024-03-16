namespace AuctionAPI_10_Api.ViewModels;

public class AuctionViewModel
{
    public long Id { get; set; }

    public ProductViewModel Product { get; set; }
    
    public DateTime StartDateTime { get; set; }

    public int DurationInSeconds { get; set; }
    
    public List<BidViewModel> Bids { get; set; }
}