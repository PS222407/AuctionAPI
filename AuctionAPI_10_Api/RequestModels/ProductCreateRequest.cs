namespace AuctionAPI_10_Api.RequestModels;

public class ProductCreateRequest
{
    public string? Name { get; set; }

    public int? PriceInCents { get; set; }

    public string? Description { get; set; }

    public IFormFile? Image { get; set; }

    public long? CategoryId { get; set; }
}