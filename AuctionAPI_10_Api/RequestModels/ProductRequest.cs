namespace AuctionAPI_10_Api.RequestModels;

public class ProductRequest
{
    public string Name { get; set; }
    
    public string Description { get; set; }

    public string? ImageUrl { get; set; }
    
    public IFormFile Image { get; set; }
}