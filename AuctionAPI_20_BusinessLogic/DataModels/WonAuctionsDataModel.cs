namespace AuctionAPI_20_BusinessLogic.DataModels;

public class WonAuctionsDataModel
{
    public int b__Id { get; set; }

    public int b__AuctionId { get; set; }

    public int b__PriceInCents { get; set; }

    public DateTime b__CreatedAt { get; set; }

    public string b__UserId { get; set; }

    public int a__id { get; set; }

    public int a__ProductId { get; set; }

    public DateTime a__StartDateTime { get; set; }

    public int a__DurationInSeconds { get; set; }

    public int p__Id { get; set; }

    public string p__Name { get; set; }

    public string p__Description { get; set; }

    public string p__ImageUrl { get; set; }

    public int p__CategoryId { get; set; }

    public bool p__ImageIsExternal { get; set; }
}