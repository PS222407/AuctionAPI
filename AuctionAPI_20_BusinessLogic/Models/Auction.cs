namespace AuctionAPI_20_BusinessLogic.Models;

public class Auction
{
    public long Id { get; set; }

    public Product Product { get; set; }

    public DateTime StartDateTime { get; set; }

    public int DurationInSeconds { get; set; }
}