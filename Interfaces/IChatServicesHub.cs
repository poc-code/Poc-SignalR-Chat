using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Interfaces
{
    public interface IChatServiceHub
    {
        Task OnConnectedAsync();
        Task OnDisconnectedAsync(Exception ex);
        Task SendMessage(string user, string message);
        Task SendMessageToGroup(string message, string group);
        Task SendPrivateMessage(string user, string message);
        Task AddToGroup(string groupName);
    }
}
