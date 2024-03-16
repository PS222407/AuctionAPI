using AuctionAPI_10_Api.Hub.Requests;
using Microsoft.AspNetCore.SignalR;

namespace AuctionAPI_10_Api.Hub;

public class MainHub : Hub<IMainHubClient>
{
    public async Task SendMessage(ChatRequest chatRequest)
    {
        await Clients.All.ReceiveMessage(chatRequest);
    }
}