namespace AuctionAPI_10_Api.ViewModels;

public class AccessTokenViewModel
{
    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public int ExpiresIn { get; set; }
}