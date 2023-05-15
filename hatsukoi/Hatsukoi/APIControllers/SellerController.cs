using Hatsukoi.Models;
using Hatsukoi.Models.Dtos;
using Hatsukoi.Models.Dtos.Seller;
using Hatsukoi.Models.ViewModels.Seller;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly SellerService _sellerService;

        public SellerController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }
        //修改Seller的資訊
        [HttpPost]
        public IActionResult EditInfo(SettingInfoDto dto)
        {
            try
            {
                _sellerService.UpdateSellerInfo(dto);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success" }
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤",
                    //Result = new List<string>()
                    Result = ex
                });
            }
        }
        //修改Seller的第二信箱資訊(寄信)
        [HttpPost]
        public IActionResult EditEmailInfo(SendResetPwdDto dto)
        {
            try
            {
                var exist = _sellerService.ExistSellerEmail(dto.InputEmail);
                if (exist)
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", hasEmail = "這個email已被使用過，請換一組" }
                    });
                }
                _sellerService.SaveSecondEmailAndTime(dto.InputEmail);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", hasEmail = "" }
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤",
                    //Result = new List<string>()
                    Result = ex
                });
            }
        }
        //修改Seller的商店介紹傳回來的資訊
        [HttpPost]
        public async Task<IActionResult> EditIntroduction(IntroductionViewModel dto)
        {
            try
            {
                if (_sellerService.ExistShopName(dto.ShopName))
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", hasShopName = "商店名稱已被使用過了，請換一組" }
                    });
                }
                await _sellerService.EditShopDesign(dto);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", hasShopName = "" }
                });
            }
            catch
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤",
                    //Result = new List<string>()
                    Result = null
                });
            }
        }
        //關閉賣家休假
        [HttpPost]
        public IActionResult CloseVacation(VacationViewModel dto)
        {
            _sellerService.CloseVaca();
            return Ok(new APIBaseResponse());
        }
        [HttpPost]
        public IActionResult OpenVacation(VacationViewModel dto)
        {
            var first = DateTime.Parse(dto.VacationFirstDay);
            var last = DateTime.Parse(dto.VacationLastDay);
            _sellerService.OpenVaca(first, last);
            return Ok(new APIBaseResponse());
        }

        [HttpPost]
        public IActionResult CreateCoupon(CouponDto dto)
        {

            try
            {
                var checkPromoCode = _sellerService.CheckPromoCode(dto.PromoCode);
                var list = _sellerService.AddCoupon(dto);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success",isCreate="已新增優惠券", userList= list }
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤",
                    //Result = new List<string>()
                    Result = ex
                });
            }
        }
        [HttpPost]
        public IActionResult checkPromoCode(CouponDto dto)
        {

            try
            {
                var checkPromoCode = _sellerService.CheckPromoCode(dto.PromoCode);
                if (checkPromoCode)
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", hasPromoCode = "此優惠碼已被使用"}
                    });
                }
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", hasPromoCode ="" }
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤",
                    //Result = new List<string>()
                    Result = ex
                });
            }
        }

        [HttpPost]
        public IActionResult DeleteCoupon(PromoCodeDto dto)
        {

            try
            {
                _sellerService.DeleteCoupon(dto.PromoCode);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success" ,isDelete="已刪除優惠券"}
                });
            }
            catch 
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤"
                });
            }
        }
        [HttpPost]
        public IActionResult UpdateCoupon(CouponDto dto)
        {

            try
            {
                _sellerService.UpdateCoupon(dto);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success" , isUpdate = "已更新優惠券" }
                });
            }
            catch
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤"
                });
            }
        }
        [HttpPost]
        public APIBaseResponse GetCouponInfo(PromoCodeDto dto)
        {

            try
            {
                var coupon=_sellerService.GetCouponByPromoCode(dto.PromoCode);
                var response = new APIBaseResponse(coupon);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤",
                    Result = ex
                };
            }
        }
        //修改賣家的財務設定
        [HttpPost]
        public IActionResult EditFinance(FinanceDto dto)
        {
            try
            {
                if (_sellerService.CheckPassCode(dto.PassCode))
                {
                    _sellerService.UpdateFinance(dto);
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success",pass = "" }
                    });
                }
                else
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", pass = "驗證碼錯誤請重新輸入" }
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤",
                    //Result = new List<string>()
                    Result = ex
                });
            }
        }
    }
}
