using Microsoft.AspNetCore.SignalR;
using server.Interfaces;
using System;
using System.Threading.Tasks;

namespace server.Service.chat
{
    public class ChatServiceHub : Hub, IChatServiceHub
    {
        private readonly IHubContext<ChatServiceHub> _client;
        private string _context;
        private string _group;

        public ChatServiceHub(IHubContext<ChatServiceHub> client)
        {
            _client = client;
            _group = "";
        }

        public override async Task OnConnectedAsync()
        {
            _context = Context.ConnectionId;
            await _client.Clients.All.SendAsync("SendAction", "Someone", "joined");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await _client.Clients.All.SendAsync("SendAction", "Someone", "left");
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await _client.Clients.Group(groupName).SendAsync("Enviar", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await _client.Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task SendPrivateMessage(string user, string message)
        {
            await _client.Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessage(string user, string message)
        {
            await _client.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToCaller(string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public async  Task SendMessageToGroup(string message, string group)
        {
            await _client.Clients.Group(group).SendAsync("ReceiveMessage", message);
        }
    }
}
