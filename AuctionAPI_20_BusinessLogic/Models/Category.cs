namespace AuctionAPI_20_BusinessLogic.Models;

public class Category
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public List<Product>? Products { get; set; }
}