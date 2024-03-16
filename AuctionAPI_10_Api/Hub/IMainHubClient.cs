using AuctionAPI_10_Api.Hub.Requests;

namespace AuctionAPI_10_Api.Hub;

public interface IMainHubClient
{
    Task ReceiveMessage(ChatRequest chatRequest);
}