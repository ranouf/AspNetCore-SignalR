using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSignalR.Api.Hubs
{
    public class TestHub : Hub
    {
        public Task Send(string data)
        {
            return Clients.All.InvokeAsync("Send", data + " (" + Context.ConnectionId + ")");
        }

        public Task SendToId(string data, string connectionId)
        {
            return Clients.Client(connectionId).InvokeAsync("SendToId", data + " (" + Context.ConnectionId + ")");
        }
    }
}
