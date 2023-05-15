using Hatsukoi.Models;
using Hatsukoi.Models.ViewModels.Cart;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hatsukoi.APIControllers
{

    public class CartController : BaseApiController
    {
        private readonly CartService _cartService;
        private readonly IAccountService _accountService;

        public CartController(CartService cartService, IAccountService accountService)
        {
            _cartService = cartService;
            _accountService = accountService;
        }
        [HttpGet]
        public APIBaseResponse Get(int userId)
        {
            try
            {
                var userCart = _cartService.GetCartByUserId(_accountService.GetUser().Id);
                var response = new APIBaseResponse(userCart);
                return response;
                //return Ok(new APIBaseResponse
                //{
                //    Status = APIStatus.Success,
                //    ErrorMsg = string.Empty,
                //    Result= userCart
                //});
            }
            catch(Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "系統錯誤",
                    Result =ex.Message
                };
            }
        }


        [HttpPost]
        public APIBaseResponse UpdateQuantity(UpdateQuantityInput input) 
        {
            try
            {
                 _cartService.UpdateQuantity(input.Quantity, input.CartId);
                var response = new APIBaseResponse() 
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "Success!"
                };
                return response;
            }
            catch(Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "系統錯誤",
                    Result = ex.Message
                };
            }
            
        }

        [HttpPost]
        public APIBaseResponse GetUserCoupon(GetCouponInput input)
        {
            try
            {
                var coupon =_cartService.GetShopCanUseCoupon(input.SellerId);
                var response = new APIBaseResponse(coupon);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "系統錯誤",
                    Result = ex.Message
                };
            }

        }

        //[HttpGet]
        //public APIBaseResponse GetCheckShop(int shopId)
        //{
        //    //改成用shopId來取
        //    try
        //    {

        //        ////使用cookieValue讀取Cookie值
        //        //string value = Request.Cookies[cookieValue];

        //        var checkShop = _cartService.GetCheckShop(shopId);
        //        var response = new APIBaseResponse(checkShop);
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIBaseResponse
        //        {
        //            Status = APIStatus.Fail,
        //            ErrorMsg = "系統錯誤",
        //            Result = ex.Message
        //        };
        //    }
        //}
    }
    

}
