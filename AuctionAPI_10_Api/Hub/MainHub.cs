using AuctionAPI_10_Api.Hub.Requests;
using Microsoft.AspNetCore.SignalR;

namespace AuctionAPI_10_Api.Hub;

public class MainHub : Hub<IMainHubClient>
{
    public async Task SendMessage(ChatRequest chatRequest)
    {
        await Clients.All.ReceiveMessage(chatRequest);
    }
    
    public async Task AddToAuctionGroup(AuctionGroupRequest auctionGroupRequest)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, auctionGroupRequest.GroupName);
    }
}