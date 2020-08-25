using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace server.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("SendAction", "Someone", "joined");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("SendAction", "Someone", "left");
        }

        public async Task SendMessage(string usuario, string mensagem)
        {
            await Clients.All.SendAsync("ReceiveMessage", usuario, mensagem);
        }
    }
}
