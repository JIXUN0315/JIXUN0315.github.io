using Coravel.Invocable;
using Hatsukoi.Hubs;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace Hatsukoi.Scheduler
{
    public class SendNotiScheduler : IInvocable
    {
        private readonly IRepository _repository;
        private readonly DateTimeOffset _currentTime;
        private readonly ILogger<SendNotiScheduler> _logger;
        private readonly IHubContext<ChatHub> _hubContext;

        public SendNotiScheduler(IRepository repository, ILogger<SendNotiScheduler> logger,IHubContext<ChatHub> hubContext)
        {
            _repository = repository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Invoke()
        {
            await SendTimeSetNoti();
        }
        private async Task SendTimeSetNoti()
        {
            List<CenterNotify> Sendlist = new List<CenterNotify>();
            Sendlist = _repository.ListWhere<CenterNotify>(x => x.IsSend == false && x.SendTime < DateTime.UtcNow);
            if (Sendlist.Count > 0)
            {
                foreach (var item in Sendlist)
                {
                    item.IsSend = true;
                    _repository.Update(item);
                    await _hubContext.Clients.All.SendAsync("CenterSend", "send");
                }
            }
        }
    }
}
