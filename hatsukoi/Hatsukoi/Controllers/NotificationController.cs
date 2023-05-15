using Hatsukoi.Models.ViewModels;
using Hatsukoi.Models.ViewModels.Notifycation;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly NotifyService _notifyService;

        public NotificationController(NotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        public IActionResult Index([FromQuery] int NotifiType=3, [FromQuery] int page=1)
        {
            _notifyService.ReadNotify();
            //取出符合條件的通知
            var notifyList = _notifyService.GetNotifyListVM(NotifiType);
            int pageSize = 5; //一頁有幾個通知
            var pagedNotifys = notifyList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var count = notifyList.Count / pageSize;
            var totalPage = notifyList.Count % pageSize == 0 ? count : count + 1;
            var VM = new NotificationListVM()
            {
                Items = pagedNotifys,
                TotalPage= totalPage,
                ThisPage= page,
                notifyType = NotifiType
            };
            return View(VM);
        }
    }
}
