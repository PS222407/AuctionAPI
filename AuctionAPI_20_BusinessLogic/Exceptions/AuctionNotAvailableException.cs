namespace AuctionAPI_20_BusinessLogic.Exceptions;

public class AuctionNotAvailableException : Exception
{
    public AuctionNotAvailableException()
    {
    }

    public AuctionNotAvailableException(string message) : base(message)
    {
    }
}