namespace AuctionAPI_10_Api.ViewModels;

public class CategoryViewModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public List<ProductViewModel> Products { get; set; }
}