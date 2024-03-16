namespace AuctionAPI_20_BusinessLogic.Exceptions;

public class BidTooLowException : Exception
{
    public BidTooLowException()
    {
    }

    public BidTooLowException(string message) : base(message)
    {
    }
}