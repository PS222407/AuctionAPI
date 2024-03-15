namespace AuctionAPI_10_Api.ViewModels;

public class ProductViewModel
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string ImageUrl { get; set; }
    
    public long? CategoryId { get; set; }
}