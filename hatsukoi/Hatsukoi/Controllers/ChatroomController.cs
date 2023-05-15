using Hatsukoi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.Controllers
{
    [Authorize]
    public class ChatroomController : Controller
    {
        private readonly ChatRoomService _chatRoomService;

        public ChatroomController(ChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        public IActionResult Index()
        {
            var VM = _chatRoomService.GetUserChatVM();
            return View(VM);
        }
        public IActionResult SellerIndex()
        {
            var VM = _chatRoomService.GetSellerChatVM();
            return View(VM);
        }
        public IActionResult UserStarChat([FromQuery]int sendTo, [FromQuery]string msg)
        {
            if (_chatRoomService.UserIsChatMyself(sendTo))
            {
                return RedirectToAction("Index","Home");
            }
            string message = string.Empty;
            if(msg != null)
            {
                message = msg;
            }
            string cookieName1 = "sendToId";
            string cookieName2 = "msg";
            string cookieValue1 = sendTo.ToString();
            string cookieValue2 = message;
            int cookieExpiration = 1; // cookie過期時間(分鐘)

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(cookieExpiration);
            option.HttpOnly = true; // 確保cookie僅透過HTTP訪問
            option.SameSite = SameSiteMode.Strict; // 設定SameSite模式
            Response.Cookies.Append(cookieName1, cookieValue1, option);
            Response.Cookies.Append(cookieName2, cookieValue2, option);
            return RedirectToAction("Index");
        }
        public IActionResult SellerStarChat([FromQuery] int sendTo, [FromQuery] string msg)
        {
            if (_chatRoomService.SellerIsChatMyself(sendTo))
            {
                return RedirectToAction("Index","Home");
            }
            string message = string.Empty;
            if (msg != null)
            {
                message = msg;
            }
            string cookieName1 = "sendToId";
            string cookieName2 = "msg";
            string cookieValue1 = sendTo.ToString();
            string cookieValue2 = message;
            int cookieExpiration = 1; // cookie過期時間(分鐘)

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(cookieExpiration);
            option.HttpOnly = true; // 確保cookie僅透過HTTP訪問
            option.SameSite = SameSiteMode.Strict; // 設定SameSite模式
            Response.Cookies.Append(cookieName1, cookieValue1, option);
            Response.Cookies.Append(cookieName2, cookieValue2, option);
            return RedirectToAction("SellerIndex");
        }
    }
}
