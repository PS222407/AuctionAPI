namespace AuctionAPI_20_BusinessLogic.Models;

public class Product
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string ImageUrl { get; set; }

    public long CategoryId { get; set; }
    
    public Category? Category { get; set; }
    
    public List<Auction> Auctions { get; set; }
}