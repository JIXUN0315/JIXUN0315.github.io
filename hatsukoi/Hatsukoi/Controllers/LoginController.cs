using Hatsukoi.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hatsukoi.Service;
using Hatsukoi.Models.ViewModels;
using System;
using Hatsukoi.Service.Interface;
using Hatsukoi.Models.Dtos.Email;
using Hatsukoi.Common;
using Hatsukoi.Models.Dtos;
using Google.Apis.Auth;
using Hatsukoi.Models;
using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Controllers
{
    public class LoginController : Controller
    {
        private readonly AccountService _accountService;
        private readonly IEmailService _emailService;

        public LoginController(AccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OpenLoginModal()
        {
            return ViewComponent("Login");
        }

        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        //當註冊時已經有第三方登入過時，導頁到讓使用者按下寄出信件按鈕來發送email並從email導頁回來綁定
        public IActionResult SendEmailToBind(int id)
        {
            var userId = HasherExtension.GetRandomNum(id);
            var email = _accountService.GetUserEmailById(userId);
            ViewData["email"] = email;
            return View();
        }
        //Email驗證連回來的網址，email驗證確認在此
        public IActionResult Certified(int id)
        {
            var result = HasherExtension.GetRandomNum(id);
            var time = _accountService.GetUserSendEmailTimeByUserId(result, HatsukoiEnum.EmailType.CheckEmail);
            var isLessThan = _accountService.IsTimeMoreThan10min(time);
            if (isLessThan)
            {
                _accountService.CertifiedEmail(result);
                return View();
            }
            else
            {
                return RedirectToAction("SendTimeOver");
            }
        }
        //重設密碼頁面渲染畫面，並且在這驗證是否開啟重設密碼欄位，沒有就倒回首頁
        public IActionResult ResetPassword(int id)
        {
            var result = HasherExtension.GetRandomNum(id);
            var time = _accountService.GetUserSendEmailTimeByUserId(result, HatsukoiEnum.EmailType.ResetPassword);
            var isLessThan = _accountService.IsTimeMoreThan10min(time);
            if (!isLessThan)
            {
                return RedirectToAction("SendTimeOver");
            }
            var userResetPwd = _accountService.GetUserIsEmailPersonalById(result);
            if (!userResetPwd)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(result);
        }
        //重設密碼成功後的跳轉畫面
        public IActionResult ResetSuccess()
        {
            return View();
        }
        //驗證時間超過10分鐘時的畫面
        public IActionResult SendTimeOver()
        {
            return View();
        }
        //登入時發生需要綁定的頁面
        public IActionResult BindPage()
        {
            BindPageDto dto = new BindPageDto();
            return View(dto);
        }

        /// <summary>
        /// 驗證 Google 登入授權
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ValidGoogleLogin()
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload payload = _accountService.VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            var idExist = _accountService.IsExternalIdExist(payload.Subject, HatsukoiEnum.ExternalLoginType.Google);
            var isEmailExist = _accountService.CheckEmail(payload.Email);
            //已有google登入過時
            if(idExist)
            {
                var userId = _accountService.GetUserIdByEmail(payload.Email);
                var user = _accountService.GetUserInfoById(userId);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, ((int)user.StoreStatus).ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Image", user.Photo),
                    new Claim("MemberLevel", ((int)user.MemberLevel).ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }
            //沒有使用google登入過，但有這個信箱註冊過時
            else if(isEmailExist)
            {
                var type = HatsukoiEnum.ExternalLoginType.Hatsukoi;
                if (_accountService.IsFbExist(payload.Email))
                {
                    type = HatsukoiEnum.ExternalLoginType.Facebook;
                }
                else if (_accountService.IsLineExist(payload.Email))
                {
                    type = HatsukoiEnum.ExternalLoginType.Line;
                }

                BindPageDto dto = new BindPageDto()
                {
                    Email = payload.Email,
                    LoginType = type,
                    RegisterType = HatsukoiEnum.ExternalLoginType.Google,
                    Token = payload.Subject
                };
                return View("BindPage", dto);
            }
            //第一 次使用這個信箱登入時
            else
            {
                _accountService.CreateGoogle(payload);
                var userId =  _accountService.GetUserIdByEmail(payload.Email);
                var user = _accountService.GetUserInfoById(userId);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, ((int)user.StoreStatus).ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Image", user.Photo),
                    new Claim("MemberLevel", ((int)user.MemberLevel).ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }
        }
        //綁定的頁面打回來的資料，如果成功綁定就自動登入
        public async Task<IActionResult> Bind(BindEmailDto dto)
        {
            if(dto.Type == "Google")
            {
                _accountService.BindGoogle(dto.Email, dto.Token);
            }
            else if(dto.Type == "Facebook")
            {

            }
            else if(dto.Type == "Line")
            {

            }
            var id =  _accountService.GetUserIdByEmail(dto.Email);
            var user = _accountService.GetUserInfoById(id);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, ((int)user.StoreStatus).ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Image", user.Photo),
                    new Claim("MemberLevel", ((int)user.MemberLevel).ToString())
                };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Home");
        }
        //當註冊時發生已使用第三方登入過，讓使用者填寫帳號密碼的頁面
        public IActionResult NormalCreateWhenExternal(int id)
        {
            var output = HasherExtension.GetRandomNum(id);
            var time = _accountService.GetUserSendEmailTimeByUserId(output, HatsukoiEnum.EmailType.BindEmail);
            var isLessThan = _accountService.IsTimeMoreThan10min(time);
            var user = _accountService.GetUserById(output);
            if (!isLessThan)
            {
                return RedirectToAction("SendTimeOver");
            }
            else if (user.IsEmailRigister == false)
            {
                return RedirectToAction("Index","Home");
            }
            var email = _accountService.GetUserEmailById(output);
            return View("NormalCreateWhenExternal", email);
        }

    }
}
