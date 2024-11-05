﻿using Microsoft.AspNetCore.SignalR;

namespace fgssr.Hubs
{
    public class AlertHub:Hub
    {
        public async Task SendAlert(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name ?? "Guest", message);
        }
    }
}
