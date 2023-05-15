using Hatsukoi.Common;
using Hatsukoi.Models.Dtos;
using Hatsukoi.Models.Dtos.Email;
using Hatsukoi.Models.Dtos.User;
using Hatsukoi.Models.ViewModels.User;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly AccountService _accountService;

        public UserController(AccountService accountService, UserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }
        //渲染User基本資料的畫面
        public async Task<IActionResult> Index()
        {
            var user = _accountService.GetUser();
            var userIndex = _userService.GetUserByUserId(user.Id);
            var thisYearPrice = _userService.GetThisYearPrice(user.Id);
            var lastYearPrice = _userService.GetLastYearPrice(user.Id);
            var userLevel = await _userService.GetThisYearLevel(thisYearPrice + lastYearPrice);
            var thisYearMission = _userService.GetYearLevel(user.Id, Common.HatsukoiEnum.WhichYear.ThisYear);
            var nextYearMission = _userService.GetYearLevel(user.Id, Common.HatsukoiEnum.WhichYear.NextYear);
            var gender = "性別未設定";
            if(userIndex.Gender == false)
            {
                gender = "女";
            }
            else if(userIndex.Gender == true)
            {
                gender = "男";
            }
            UserIndexViewModel VM = new UserIndexViewModel()
            {
                UserName = user.Name,
                UserLevel = userLevel,
                ThisYearMissionName = thisYearMission.Level,
                NextYearMissionName = nextYearMission.Level,
                ThisYearMissionPrice = thisYearMission.price,
                NextYearMissionPrice = nextYearMission.price,
                UserImg = user.Image,
                UserGender = gender,
                UserCreateTime = userIndex.CreateDate
            };
            return View(VM);
        }
        public IActionResult Address()
        {
            return View();
        }
        //渲染UserCoupon畫面
        public IActionResult Coupon()
        {
            var user = _accountService.GetUser();
            var VM = _userService.GetUserCouponViewModel(user.Id);
            return View(VM);
        }
        public IActionResult Credit()
        {
            return View();
        }
        //渲染Email通知設定畫面
        public IActionResult Email()
        {
            var VM = _userService.GetEmailNotifiVM();
            return View(VM);
        }
        //渲染UserOrder畫面
        public IActionResult Order()
        {
            var user = _accountService.GetUser();
            var AllList = new UserAllOrder()
            {
                NotPayList = _userService.GetUserStatusOrders(Common.HatsukoiEnum.OrderStatus.NotPay, user.Id),
                NotShippedList = _userService.GetUserStatusOrders(Common.HatsukoiEnum.OrderStatus.NotShipped, user.Id),
                ToBeReceivedList = _userService.GetUserStatusOrders(Common.HatsukoiEnum.OrderStatus.ToBeReceived, user.Id),
                CompletedList = _userService.GetUserStatusOrders(Common.HatsukoiEnum.OrderStatus.Completed, user.Id),
                CancelledList = _userService.GetUserStatusOrders(Common.HatsukoiEnum.OrderStatus.Cancelled, user.Id),
            };
            return View(AllList);
        }
        //渲染UserOrderDetail畫面
        public IActionResult OrderDetail([FromQuery] string orderNum)
        {
            var orderDetail = _userService.GetUserOrder(orderNum);
            return View(orderDetail);
        }
        //渲染User資料設定畫面
        public IActionResult Setting()
        {
            var user = _accountService.GetUser();
            var VM = _userService.GetUserSettingViewModel(user.Id);
            return View(VM);
        }
        //申請成為賣家的畫面
        public IActionResult BeSeller()
        {
            return View();
        }
        //申請成為賣家填寫表單完的畫面
        public IActionResult Finish()
        {
            return View();
        }
        //修改Email信件連回來的畫面
        public IActionResult ChangeEmail(int id)
        {
            var result = HasherExtension.GetRandomNum(id);
            var user = _userService.GetUserByUserId(result);
            var time = _accountService.GetUserSendEmailTimeByUserId(result, HatsukoiEnum.EmailType.ChangeEmail);
            var isLessThan = _accountService.IsTimeMoreThan10min(time);
            if(user.NewEmail == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (isLessThan)
            {
                //修改email
                _userService.UpdateEmailFromNewEmail(user);
                return View();
            }
            else
            {
                return RedirectToAction("SendTimeOver","Login");
            }
        }
    }
}
