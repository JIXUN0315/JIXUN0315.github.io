using Hatsukoi.Service.Interface;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hatsukoi.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Hatsukoi.Models;
using Hatsukoi.Common;
using Hatsukoi.Models.Dtos.Email;
using Hatsukoi.Models.ViewModels;
using System;
using Hatsukoi.Models.Dtos;
using Hatsukoi.Repository.EntityModel;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly IEmailService _emailService;
        public LoginController(AccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }
        //登入，必須確認user的email有沒有驗證
        [HttpPost]
        public async Task<IActionResult> login(LoginViewModel loginModel)
        {
            try
            {
                var user = _accountService.GetUserLoginModel(loginModel);
                //登入後的倒頁
                var referer = Request.Headers.Referer.ToString();
                var origin = Request.Headers.Origin.ToString();
                //登入功能
                if (user.Id == 0)
                {
                    //return Ok(new { result = "fail", Action = "" });
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Fail,
                        ErrorMsg = "登入失敗，找不到此User",
                        Result = new { result = "fail", controller = "", isEmailCertified = "" }
                    });
                }
                else if (!user.IsEmailCertified)
                {
                    //return Ok(new { result = "fail", Action = "", isEmailCertified = "no" });
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Fail,
                        ErrorMsg = "登入失敗，找不到此User",
                        Result = new { result = "fail", controller = "", isEmailCertified = "no" }
                    });
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, ((int)user.StoreStatus).ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Image", user.Image),
                    new Claim("MemberLevel", ((int)user.MemberLevel).ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = string.Empty,
                    Result = new { result = "success",  controller = referer}
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
        public IActionResult Create(CreateUserViewModel CreateModel)
        {
            try
            {
                DateTime date;
                date = new DateTime(CreateModel.Year, CreateModel.Month, CreateModel.Day);
                _accountService.CreateUser(CreateModel);
                var id = _accountService.GetUserIdByEmail(CreateModel.Email);
                EmailDto email = new EmailDto()
                {
                    Email = CreateModel.Email,
                    EmailType = Common.HatsukoiEnum.EmailType.CheckEmail,
                    Id = id,
                    Account = _accountService.GetUserAccountById(id)
                };
                _emailService.Send(email);
                _accountService.UpdateUserSendEmailTimeByUserId(id, HatsukoiEnum.EmailType.CheckEmail);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = string.Empty,
                    Result = new { result = "success", IsSuccess = true, address = CreateModel.Email }
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
        //寄密碼重設信，並且開啟重設密碼欄位，否則倒回首頁
        [HttpPost]
        public IActionResult SendResetPwd(SendResetPwdDto dto)
        {
            try
            {
                var userId = _accountService.GetUserIdByEmail(dto.InputEmail);
                if (userId == 0)
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Fail,
                        ErrorMsg = "沒有開啟重設密碼的服務",
                        //Result = new List<string>()
                        Result = new { result = "success", hasEmail = "" }
                    });
                }
                _accountService.OpenResetPwdAddress(userId);
                EmailDto email = new EmailDto()
                {
                    Email = dto.InputEmail,
                    EmailType = Common.HatsukoiEnum.EmailType.ResetPassword,
                    Id = userId,
                    Account = _accountService.GetUserAccountById(userId)
                };
                _emailService.Send(email);
                _accountService.UpdateUserSendEmailTimeByUserId(userId, HatsukoiEnum.EmailType.ResetPassword);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", hasEmail = "has" }
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
        //重設密碼頁面接收重新設定的密碼
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                var userAccount = _accountService.GetUserAccountById(dto.id);
                var output = _accountService.CertifiedAccount(userAccount, dto);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", rightAccount = output }
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
        //重寄Email驗證信
        [HttpPost]
        public IActionResult ResendEmail(SendResetPwdDto dto)
        {
            try
            {
                var userId = _accountService.GetUserIdByEmail(dto.InputEmail);
                if (userId == 0)
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Fail,
                        ErrorMsg = "查無此 email !",
                        //Result = new List<string>()
                        Result = new { result = "success", hasEmail = "查無此 email !" }
                    });
                }
                EmailDto email = new EmailDto()
                {
                    Email = dto.InputEmail,
                    EmailType = Common.HatsukoiEnum.EmailType.CheckEmail,
                    Id = userId,
                    Account = _accountService.GetUserAccountById(userId)
                };
                _emailService.Send(email);
                _accountService.UpdateUserSendEmailTimeByUserId(userId, HatsukoiEnum.EmailType.CheckEmail);
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
        //註冊時檢查email與account是否被註冊過
        [HttpPost]
        public async Task<IActionResult> CheckEmail(CheckEmailViewModel check)
        {
            try
            {
                var result =  await _accountService.CheckEmailAndAccount(check);
                if (result.CheckEmail && (_accountService.IsFbExist(check.Email) || _accountService.IsGoogleExist(check.Email) || _accountService.IsLineExist(check.Email)))
                {
                    var a = _accountService.GetUserAccountById(_accountService.GetUserIdByEmail(check.Email));
                    if (a != "null")
                    {
                        return Ok(new APIBaseResponse
                        {
                            Status = APIStatus.Success,
                            ErrorMsg = "",
                            //Result = new List<string>()
                            Result = new { result = "success", email = result.CheckEmail, account = result.CheckAccount, myId = "" }
                        });
                    }
                    var id = _accountService.GetUserIdByEmail(check.Email);
                    var randomNum = HasherExtension.SetRandomNum(id);
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Fail,
                        ErrorMsg = "已經有過第三方登入",
                        //Result = new List<string>()
                        Result = new { result = "success", myId = randomNum.ToString() }
                    });
                }
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", email = result.CheckEmail, account = result.CheckAccount, myId = "" }
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
        //當註冊時已經有第三方登入過時，導頁到讓使用者按下寄出信件按鈕來發送email並從email導頁回來綁定，按下確認後打回來的資料以此寄出email
        [HttpPost]
        public IActionResult SendEmailBind(SendResetPwdDto dto)
        {
            try
            {
                var id = _accountService.GetUserIdByEmail(dto.InputEmail);
                _accountService.OpenEmailBind(id);
                EmailDto email = new EmailDto()
                {
                    Email = dto.InputEmail,
                    EmailType = Common.HatsukoiEnum.EmailType.BindEmail,
                    Id = id
                };
                _emailService.Send(email);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { }
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
        //遇到先有第三方登入時，創建專屬Hatsukoi帳密打回來的資料
        public IActionResult RegisterHatsukoi(RegisterHatsukoiDto dto)
        {
            try
            {
                if (_accountService.IsAccountExist(dto.Account))
                {
                    return Ok(new APIBaseResponse
                    {
                        Status = APIStatus.Success,
                        ErrorMsg = "",
                        //Result = new List<string>()
                        Result = new { result = "success", rightAccount = "帳號已存在，請換一組" }
                    });
                }
                _accountService.UserSetAccountAndPassword(dto);
                var output = _accountService.GetUserIdByEmail(dto.Email);
                _accountService.CloseEmailBund(output);
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = "",
                    //Result = new List<string>()
                    Result = new { result = "success", rightAccount = "" }
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
