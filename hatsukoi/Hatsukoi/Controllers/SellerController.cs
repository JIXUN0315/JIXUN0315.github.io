using Hatsukoi.Common;
using Hatsukoi.Models.Dtos.Seller;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.Controllers
{
    [Authorize(Roles = "3")]
    public class SellerController : Controller
    {
        private readonly OrderService _orderService;
        private readonly SellerService _sellerService;
        private readonly AccountService _accountService;

        public SellerController(SellerService sellerService, AccountService accountService, OrderService orderService)
        {
            _sellerService = sellerService;
            _accountService = accountService;
            _orderService = orderService;
        }


        //渲染賣家基本資訊
        public IActionResult InfoSetting()
        {
            var VM = _sellerService.GetSettingViewModel();
            return View(VM);
        }
        //賣家第二信箱驗證信連回來的網址
        public IActionResult UpdateSecondEmail(int id)
        {
            var result = HasherExtension.GetRandomNum(id);
            var seller = _sellerService.GetSellerBySellerId(result);
            var time = (DateTime)seller.StateSecondTime;
            var isLessThan = _accountService.IsTimeMoreThan10min(time);
            if (isLessThan)
            {
                _sellerService.UpdateSecondEmail(seller);
                return View();
            }
            else
            {
                return RedirectToAction("SendTimeOver", "Login");
            }
        }
        //渲染賣家休假設定葉面
        public IActionResult Vacation()
        {
            var VM = _sellerService.GetVacationVM();
            return View(VM);
        }
        //渲染賣家商家設定
        public IActionResult Introduction()
        {
            var VM = _sellerService.GetIntroductionVM();
            return View(VM);
        }
        //渲染財務資訊的設定
        public IActionResult Finance()
        {
            var VM = _sellerService.GetFinanceVM();
            return View(VM);
        }


        public IActionResult CouponSetting()
        {
           var userId= _accountService.GetUser().Id;
           var sellerId= _sellerService.GetSellerIdByUserId (userId);
            var couponList = _sellerService.GetCouponBySellerId(sellerId);
            return View(couponList);
        }
        public IActionResult SellerHome()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }


    }
}
