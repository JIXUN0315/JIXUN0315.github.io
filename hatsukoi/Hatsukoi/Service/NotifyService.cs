using Hatsukoi.Models.ViewModels.Notifycation;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Hatsukoi.Service
{
    public class NotifyService
    {
        private readonly IRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly AccountService _accountService;

        public NotifyService(IRepository repository, AccountService accountService,IUserRepository userRepository)
        {
            _repository = repository;
            _accountService = accountService;
            _userRepository = userRepository;
        }
        public List<NotificationViewModel> GetNotifyListVM(int kindOfNotifi)
        {
            var now = DateTime.UtcNow;
            var userId = _accountService.GetUser().Id;
            var user = _repository.FirstOrDefault<User>(x => x.Id == userId);
            var normalNoti = new List<Notification>();
            var centerNoti = new List<CenterNotify>();
            //訂單通知只需顯示訂單通知
            if(kindOfNotifi == 2)
            {
                var notifyList = _repository.ListWhere<Notification>(x => x.SendTo == userId && x.KindOfNotifi == kindOfNotifi).OrderByDescending(x => x.SendTime).Select(x => new NotificationViewModel()
                {
                    Id = x.Id,
                    SendTime = x.SendTime.AddHours(8).ToString("F"),
                    Text = x.Text,
                    Title = x.Title,
                    LinkTo = x.LinkTo,
                    SendImg = x.SendImg
                }).ToList();
                return notifyList;
            }
            //帳號通知必須有優惠券通知和後台通知
            //先取出紙給User的通知
            if(kindOfNotifi == 3)
            {
                normalNoti = _repository.ListWhere<Notification>(x => x.SendTo == userId);
            }
            else
            {
                normalNoti = _repository.ListWhere<Notification>(x => x.SendTo == userId && x.KindOfNotifi == kindOfNotifi);
            }
            //再取出後台給所有User的通知
            centerNoti = _repository.ListWhere<CenterNotify>(x => x.SendTime > user.CreateDate && x.IsSend == true);

            if(centerNoti == null)
            {
                var notifyListAll = _repository.ListWhere<Notification>(x => x.SendTo == userId).OrderByDescending(x => x.SendTime).Select(x => new NotificationViewModel()
                {
                    Id = x.Id,
                    SendTime = x.SendTime.AddHours(8).ToString("F"),
                    Text = x.Text,
                    Title = x.Title,
                    LinkTo = x.LinkTo,
                    SendImg = x.SendImg
                }).ToList();
                return notifyListAll;
            }
            var normalList = normalNoti.Select(x => new NotificationOrderDto { Id = x.Id, SendTime = x.SendTime, Text = x.Text, Title = x.Title, LinkTo = x.LinkTo, SendImg = x.SendImg });
            var centerList = centerNoti.Select(x => new NotificationOrderDto { Id = x.Id, SendTime = x.SendTime, Text = x.Text, Title = x.Title, LinkTo = x.LinkTo, SendImg = x.SendImg });
            var allList = normalList.Concat(centerList);
            var VM = allList.OrderByDescending(x => x.SendTime).Select(x => new NotificationViewModel()
            {
                Id = x.Id,
                SendTime = x.SendTime.AddHours(8).ToString("F"),
                Text = x.Text,
                Title = x.Title,
                LinkTo = x.LinkTo,
                SendImg = x.SendImg
            }).ToList();
            return VM;
        }
        public void ReadNotify()
        {
            var userId = _accountService.GetUser().Id;
            var user = _repository.FirstOrDefault<User>(x => x.Id == userId);
            user.LookNotifyTime = DateTime.UtcNow;
            _repository.Update(user);
        }
    }
}
