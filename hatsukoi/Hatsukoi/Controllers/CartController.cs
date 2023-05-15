using Hatsukoi.Service.Interface;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Mvc;
using Hatsukoi.Models.Dtos;
using Hatsukoi.Models.ViewModels.Cart;
using Hatsukoi.Repository.EntityModel;
using Microsoft.AspNetCore.Authorization;

namespace Hatsukoi.Controllers
{
    [Authorize]
    public class CartController : Controller
    {


        private readonly CartService _cartService;
        private readonly IAccountService _accountService;
        private readonly PaymentService _paymentService;


        public CartController(CartService cartService, IAccountService accountService, PaymentService paymentService)
        {
            _cartService = cartService;
            _accountService = accountService;
            _paymentService = paymentService;
        }
        public IActionResult Index()
        {
            //購物車第一頁
            //var userCart = _cartService.GetCartByUserId(_accountService.GetUser().Id);
            //ViewData["userCart"] = userCart;
            //return View();
            return View();
        }
       
        [HttpPost]
        public IActionResult CartToCheck(ShopCart checkShop)
        {
            //購物車第二頁
            return View();
        }
        //[HttpPost]
        public async Task<IActionResult> AddCart(int productId, int specId, int quantity) 
        {
            var user = _accountService.GetUser().Id;
            var msg = await _cartService.CreateCartList(productId, user, specId , quantity);
            if (msg == "save")
            {
                //await _cartService.CreateCartList(productId, _accountService.GetUser().Id, specId, quantity);
                return Json(new { result = "success", IsCreate = "save" });
            }
            else
            {
                //_cartService.CreateCartList(productId, _accountService.GetUser().Id);

                return Json(new { result = "success", IsCreate = "" });
                
            }
            
        }

        public IActionResult DeleteCartProduct(int id)
        {
            var user = _accountService.GetUser().Id;
            var msg = _cartService.DeleteCartProduct(id, user);

            if(msg == "delete")
            {
                _cartService.DeleteCartProduct(id, user);   
                return Json(new { result = "success", IsDelete = "delete" });
            }
            else
            {
                return Json(new { result = "success", IsDelete = "" });
            }

        }

        public IActionResult DeleteCartShop(int id)
        {
            var user = _accountService.GetUser().Id;
            var msg = _cartService.DeleteCartShop(id, user);

            if (msg == "delete")
            {
                _cartService.DeleteCartShop(id, user);
                return Json(new { result = "success", IsDelete = "delete"});
            }
            else
            {
                return Json(new { result = "success", IsDelete = "" });

            }
        }

        [HttpPost]
        public IActionResult GetCheckShop([FromForm]GetCheckShopInput input)
        {
            //在第一頁取得shopId傳入此action
            //用此shopId取得第二頁要渲染的model
            var checkShop = _cartService.GetCheckShop(input.ShopId , input.CouponId, input.DiscountAmount);


            return View("CartToCheck",checkShop);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderViewModel input)
        {
            var user = _accountService.GetUser().Id;
            try
            {
                //_cartService.CreateOrderAndOrderDetail(_accountService.GetUser().Id, input);
                var orderId = _paymentService.CreateOrderAndOrderDetail(user, input);
                return Json(new { result = "success", IsCreate = "", OrderId = orderId});
            }
            catch (Exception ex)
            {
                return Json(new { result = "success", IsCreate = "fail" });
            }
        
        }

        public IActionResult PayComplete()
        {
            return View();
        }

    }
    
}
