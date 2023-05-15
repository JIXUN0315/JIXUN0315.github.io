using Hatsukoi.Hubs;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Hatsukoi.Service
{
    public class CenterNotificationService:ICenterNotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public CenterNotificationService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendMsg()
        {
            await _hubContext.Clients.All.SendAsync("CenterSend", "send");
        }
    }
}
