using Hatsukoi.Models;
using Hatsukoi.Models.Dtos;
using Hatsukoi.Models.Dtos.User;
using Hatsukoi.Repository.Migrations;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController] 
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AccountService _accountService;

        public UserController(AccountService accountService, UserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }
        //接收Email通知設定畫面回傳的更新資訊
        [HttpPost]
        public IActionResult EditEmailNotifi(EmailNotifiDto input)
        {
            try
            {
                _userService.UpdateEmailNotifi(input);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success"}
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
        //修改User資料傳回來的資料
        [HttpPost]
        public async Task<IActionResult> EditInfo(EditSettingDto input)
        {
            try
            {
                var img = await _userService.GetUserImg(input.Img);
                var userid = _accountService.GetUser().Id;
                var user = _userService.GetUserByUserId(userid);
                user.Photo = img;
                user.Name = input.Name;
                user.Gender = input.Gender == 1;
                DateTime birth = DateTime.Parse(input.Birthday);
                user.BirthDate = birth;
                _userService.EditUser(user);
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
        [HttpPost]
        public IActionResult EditEmail(SendResetPwdDto dto)
        {
            try
            {
                var googleExist = _userService.ExistGoogle();
                if (googleExist)
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", hasGoogle = "已經有綁定Google登入，無法更換Email", hasEmail = "" }
                    });
                }
                var hasEmail = _accountService.CheckEmail(dto.InputEmail);
                if (hasEmail)
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", hasEmail = "這組Email已經被註冊過了" , hasGoogle = "" }
                    });
                }
                //更新新的Email近準備修改email欄位
                _userService.UpdateNewEmail(dto.InputEmail);
                //寄信
                _userService.SendChangeEmail(dto.InputEmail);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", hasEmail = "", hasGoogle = "" }
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
        //申請成為賣家傳回來的資料
        [HttpPost]
        public async Task<IActionResult> ApplySeller(BeSellerDto dto)
        {
            try
            {
                if (await _userService.ExistSellerEmail(dto.Email))
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", warn = "email已被使用過" }
                    });
                }
                else if (await _userService.ExistShopName(dto.ShopName))
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", warn = "商店名稱已被使用過" }
                    });
                }
                var user = _accountService.GetUser();
                //寄信
                await _userService.SendApplyEmail(dto, user.Id);

                //創建Seller(之後在後台需要審核)
                //_userService.SendSuccessEmail(dto, user.Id);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", warn = "" }
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
        //完成訂單的按鈕按下後要將狀態改為完成
        [HttpPost]
        public IActionResult FinishOrder(SendResetPwdDto dto)
        {
            try
            {
                _userService.FinishOrderByOrderNumber(dto.InputEmail);
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
        //送出訂單評價
        [HttpPost]
        public IActionResult EvaluateOrder(EvaluateDto dto)
        {
            try
            {
                _userService.SetEvaluate(dto);
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
    }
}
