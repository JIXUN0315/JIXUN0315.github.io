using Hatsukoi.Models;
using Hatsukoi.Models.Dtos.ChatDto;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatroomController : ControllerBase
    {
        private readonly ChatRoomService _chatroomService;

        public ChatroomController(ChatRoomService chatroomService)
        {
            _chatroomService = chatroomService;
        }
        //取得買家的訊息導覽列
        [HttpGet]
        public APIBaseResponse GetMessages()
        {
            try
            {
                var messageList = _chatroomService.GetUserChatRoomVM();
                var response = new APIBaseResponse(messageList);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //取得買家的聊天訊息紀錄
        [HttpPost]
        public APIBaseResponse SelectMessage(SelectMsgDto dto)
        {
            try
            {
                var messageList = _chatroomService.GetMessageDetailViewModels(dto.ReceiverId);
                var response = new APIBaseResponse(messageList);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //取得賣家的訊息導覽列
        [HttpGet]
        public APIBaseResponse GetSellerMessages()
        {
            try
            {
                var messageList = _chatroomService.GetSellerChatRoomVM();
                var response = new APIBaseResponse(messageList);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //取得賣家的聊天訊息紀錄
        [HttpPost]
        public APIBaseResponse SelectSellerMessage(SelectMsgDto dto)
        {
            try
            {
                var messageList = _chatroomService.GetSellerMessageDetailViewModels(dto.ReceiverId);
                var response = new APIBaseResponse(messageList);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //開啟聊天時把預設值船進去畫面
        [HttpPost]
        public APIBaseResponse UserStar()
        {
            try
            {
                string sendTo = Request.Cookies["sendToId"];
                string msg = Request.Cookies["msg"];
                if(sendTo == null || msg == null)
                {
                    var fail = new APIBaseResponse("");
                    return fail;
                }
                var star = _chatroomService.GetStar(sendTo,msg);
                var response = new APIBaseResponse(star);
                Response.Cookies.Delete("sendToId");
                Response.Cookies.Delete("msg");
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //開啟聊天時把預設值船進去畫面
        [HttpPost]
        public APIBaseResponse SellerStar()
        {
            try
            {
                string sendTo = Request.Cookies["sendToId"];
                string msg = Request.Cookies["msg"];
                if (sendTo == null || msg == null)
                {
                    var fail = new APIBaseResponse("");
                    return fail;
                }
                var star = _chatroomService.GetSellerStar(sendTo, msg);
                var response = new APIBaseResponse(star);
                Response.Cookies.Delete("sendToId");
                Response.Cookies.Delete("msg");
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //滾輪往上去找更舊的五筆聊天資訊
        [HttpPost]
        public APIBaseResponse UserGet5Message(ShowOlderMsgDto dto)
        {
            try
            {
                var msgs = _chatroomService.UserGet_5_OlderMsg(dto.MsgCount, dto.ReceiverId);
                var response = new APIBaseResponse(msgs);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //賣家滾輪往上去找更舊的五筆聊天資訊
        [HttpPost]
        public APIBaseResponse SellerGet5Message(ShowOlderMsgDto dto)
        {
            try
            {
                var msgs = _chatroomService.SellserGet_5_OlderMsg(dto.MsgCount, dto.ReceiverId);
                var response = new APIBaseResponse(msgs);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //買家聊天中把訊息已讀
        [HttpPost]
        public APIBaseResponse UserReadMessage(SelectMsgDto dto)
        {
            try
            {
                _chatroomService.UserRaedMsg(dto.ReceiverId);
                var response = new APIBaseResponse();
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //賣家聊天中把訊息已讀
        [HttpPost]
        public APIBaseResponse SellerReadMessage(SelectMsgDto dto)
        {
            try
            {
                _chatroomService.SellerRaedMsg(dto.ReceiverId);
                var response = new APIBaseResponse();
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //取得賣家首頁訊息的未讀總數
        [HttpGet]
        public APIBaseResponse SellerGetUnReadCount()
        {
            try
            {
                var count = _chatroomService.GetSellerUnRead();
                var response = new APIBaseResponse(count);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
        //寄送照片功能
        [HttpPost]
        public async Task<APIBaseResponse> SendImg(ImgDto dto)
        {
            try
            {
                var img = await _chatroomService.GetImgUrl(dto.Img);
                var response = new APIBaseResponse(img);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
    }
}
