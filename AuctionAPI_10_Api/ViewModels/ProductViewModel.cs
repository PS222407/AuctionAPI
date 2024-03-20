namespace AuctionAPI_10_Api.ViewModels;

public class ProductViewModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public CategoryViewModel? Category { get; set; }

    public List<AuctionViewModel> Auctions { get; set; }
}