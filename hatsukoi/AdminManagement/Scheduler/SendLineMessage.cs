using AdminManagement.Models.ViewModels;
using AdminManagement.Services;
using Coravel.Invocable;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.AspNetCore.SignalR;

namespace AdminManagement.Scheduler
{
    public class SendLineMessage : IInvocable
    {
        private readonly IRepository _repository;
        private readonly LineBotService _lineBotService;

        public SendLineMessage(IRepository repository, LineBotService lineBotService)
        {
            _repository = repository;
            _lineBotService = lineBotService;
        }

        public Task Invoke()
        {
            SendTimeSetMessage();
            return Task.CompletedTask;
        }

        private void SendTimeSetMessage()
        {
            List<LineMessage> Sendlist = new List<LineMessage>();
            Sendlist = _repository.ListWhere<LineMessage>(x => x.IsSend == false && x.SendTime < DateTime.UtcNow);
            if (Sendlist.Count > 0)
            {
                foreach (var item in Sendlist)
                {
                    item.IsSend = true;
                    _repository.Update(item);
                    var lineCreateVM = new LineCreateViewModel
                    {
                        Text = item.Text
                    };
                    _lineBotService.BoardcastMessage(lineCreateVM);
                }
            }
        }
    }
}
