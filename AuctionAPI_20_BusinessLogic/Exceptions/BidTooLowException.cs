namespace AuctionAPI_20_BusinessLogic.Exceptions;

public class BidTooLowException(string message) : Exception(message);