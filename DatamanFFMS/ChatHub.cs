using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AstralFFMS
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Send(string name, string message)
        {
            Clients.All.sendMessage(name, message);
        }
    }
}