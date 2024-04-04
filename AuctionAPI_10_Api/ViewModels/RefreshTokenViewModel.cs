namespace AuctionAPI_10_Api.ViewModels;

public class RefreshTokenViewModel
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public string TokenType { get; set; }

    public int ExpiresIn { get; set; }
}