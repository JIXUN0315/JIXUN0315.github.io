using Hatsukoi.Models;
using Hatsukoi.Models.ViewModels.ChatRoom;
using Hatsukoi.Models.ViewModels.Notifycation;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;
using OpenAI_API.Completions;
using OpenAI_API;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Hatsukoi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IRepository _repository;
        private readonly AccountService _accountService;

        public ChatHub(IRepository repository, AccountService accountService)
        {
            _repository = repository;
            _accountService = accountService;
        }
        //人員線上清單
        public static List<ChatOnline> UserList = new List<ChatOnline>();

        /// <summary>
        /// 上線
        /// </summary>
        /// <returns></returns>
        public async override Task OnConnectedAsync()
        {
            var user = _accountService.GetUser();
            if (user != null)
            {
                var userId = user.Id;
                //檢查這個連線id是否沒被使用，沒被使用過才把這個id家進資料庫
                if (!await _repository.AnyAsync<ChatOnline>(x => x.ConnectionId == Context.ConnectionId))//Context網路連線相關
                {
                    //當這個User有在別的地方上限時，要讓他原本上限的地方地方下線(刪掉舊的連線id)，並且從這裡上限(加入新的連線id)
                    if (await _repository.AnyAsync<ChatOnline>(x => userId == x.UserId))
                    {
                        var chatUser = _repository.FirstOrDefault<ChatOnline>(x => x.UserId == userId);
                        _repository.Delete(chatUser);
                    }
                    var newChatUser = new ChatOnline()
                    {
                        UserId = userId,
                        ConnectionId = Context.ConnectionId,
                    };
                    _repository.Create(newChatUser);
                }
                var readCount = _repository.ListWhere<Chatroom>(x => x.UserId == userId && x.IsRead == 2 && x.MessageType == 2).Count;
                await Clients.Client(Context.ConnectionId).SendAsync("getUser", readCount);
                var now = DateTime.UtcNow;
                var theUser = _repository.FirstOrDefault<User>(x => x.Id == userId);
                int normalNotiRead = 0;
                int centerNotiRead = 0;
                if (theUser.LookNotifyTime != null)
                {
                    normalNotiRead = _repository.ListWhere<Notification>(x => x.SendTo == userId && x.SendTime > theUser.LookNotifyTime).Count;
                    centerNotiRead = _repository.ListWhere<CenterNotify>(x => x.SendTime > theUser.LookNotifyTime && x.IsSend == true).Count;
                }
                else
                {
                    normalNotiRead = _repository.ListWhere<Notification>(x => x.SendTo == userId && x.SendTime > theUser.LookNotifyTime).Count;
                    centerNotiRead = _repository.ListWhere<CenterNotify>(x => x.SendTime > theUser.CreateDate && x.IsSend == true).Count;
                }
                var notiReadCount = normalNotiRead + centerNotiRead;
                await Clients.Client(Context.ConnectionId).SendAsync("getNoti", notiReadCount);
                await base.OnConnectedAsync();
                UserList = _repository.ListWhere<ChatOnline>(x => true);
            }
        }
        /// <summary>
        /// 下線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (await _repository.AnyAsync<ChatOnline>(x => x.ConnectionId == Context.ConnectionId))
            {
                var userId = _accountService.GetUser().Id;
                var chatUser = _repository.FirstOrDefault<ChatOnline>(x => x.UserId == userId);
                _repository.Delete(chatUser);
            }
            await base.OnDisconnectedAsync(exception);
            UserList = _repository.ListWhere<ChatOnline>(x => true);
        }
        //User寄訊息，讓買家畫面顯示
        //賣家畫面較複雜，要先判斷是否正在那個對話視窗，如果是的話顯示於畫面，如果不是的話顯示在左側訊息預覽
        public async Task UserSendMessage(int receiverSellerId, string message)
        {
            var now = DateTime.UtcNow;
            var userId = _accountService.GetUser().Id;
            var sellerUserId = _repository.FirstOrDefault<Seller>(x => x.Id == receiverSellerId).UserId;
            var user = _accountService.GetUserById(userId);
            //取得賣家資訊(以防他們沒有聊天紀錄過)
            var seller = _repository.FirstOrDefault<Seller>(x => x.Id == receiverSellerId);
            //發送者的畫面
            var sender = new ChatViewModel()
            {
                ReceiverImg = seller.Icon,
                ReceiverName = seller.ShopName,
                ReceiverId = seller.Id,
                LastMessage = message,
                LastMessageTime = now.AddHours(8).ToString("HH:mm")
            };
            await Clients.Client(Context.ConnectionId).SendAsync("userSendMsgBox", sender);
            if (receiverSellerId != 27)
            {
                //當使用者在線上時
                if (UserList.Any(x => x.UserId == sellerUserId))
                {
                    var reciver = new ChatViewModel()
                    {
                        ReceiverImg = user.Photo,
                        ReceiverName = user.Name,
                        ReceiverId = userId,
                        LastMessage = message,
                        LastMessageTime = now.AddHours(8).ToString("HH:mm")
                    };
                    //取得賣家的ConnectionId
                    var sendto = UserList.FirstOrDefault(x => x.UserId == sellerUserId).ConnectionId;
                    //接收訊息者(賣家)的畫面
                    await Clients.Client(sendto).SendAsync("sellerUpdateMsgBox", reciver);
                    await Clients.Client(sendto).SendAsync("sellerLayout", reciver);
                }
            }
            else
            {
                string sendToGpt = "你是Hatsukoi電商的聊天機器人，請給顧客適當的回答，且直接回答以下的問題:" + message;
                string apiKey = "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";
                string answer = string.Empty;
                var openai = new OpenAIAPI(apiKey);
                CompletionRequest completion = new CompletionRequest();
                completion.Prompt = sendToGpt;
                completion.MaxTokens = 3000;
                try
                {
                    var result = await openai.Completions.CreateCompletionAsync(completion);
                    if (result != null)
                    {
                        foreach (var item in result.Completions)
                        {
                            answer = item.Text;
                        }
                        await GptSendToUser(userId, answer);
                    }
                    else
                    {
                        await GptSendToUser(userId, "非常抱歉您的回答超出了我的範圍，您可以換個問題詢問我，像是: 請給我Hatsukoi好商品");
                    }
                }
                catch(Exception ex)
                {
                    await GptSendToUser(userId, "非常抱歉您的回答超出了我的範圍，您可以換個問題詢問我，像是: 請推薦好商品");
                }
            }
            //將訊息記錄存進資料庫
            var newMessage = new Chatroom()
            {
                CreateTime = now,
                UserId = userId,
                SellerId = receiverSellerId,
                Message = message,
                MessageType = 1,
                IsRead = 2
            };
            _repository.Create(newMessage);
        }
        //GPT寄訊息給買家
        public async Task GptSendToUser(int receiverUserId, string gptReply)
        {
            var message = gptReply.Substring(1);
            var now = DateTime.UtcNow;
            var seller = _repository.FirstOrDefault<Seller>(x => x.Id == 27);
            var user = _repository.FirstOrDefault<User>(x => x.Id == receiverUserId);
            if (UserList.Any(x => x.UserId == receiverUserId))
            {
                var reciver = new ChatViewModel()
                {
                    ReceiverImg = seller.Icon,
                    ReceiverName = seller.ShopName,
                    ReceiverId = seller.Id,
                    LastMessage = message,
                    LastMessageTime = now.AddHours(8).ToString("HH:mm")
                };
                //取得買家的ConnectionId
                var sendto = UserList.FirstOrDefault(x => x.UserId == receiverUserId).ConnectionId;
                //接收訊息者(買家)的畫面
                await Clients.Client(sendto).SendAsync("userUpdateMsgBox", reciver);
                await Clients.Client(sendto).SendAsync("userLayout", reciver);
                var newMessage = new Chatroom()
                {
                    CreateTime = now,
                    UserId = receiverUserId,
                    SellerId = seller.Id,
                    Message = message,
                    MessageType = 2,
                    IsRead = 2
                };
                _repository.Create(newMessage);
            }
        }
        //Seller寄訊息
        public async Task SellerSendMessage(int receiverUserId, string message)
        {
            //var sellerUserId = _repository.FirstOrDefault<Seller>(x => x.Id == receiverUserId).UserId;
            var now = DateTime.UtcNow;
            //賣家自己的UserId
            var userId = _accountService.GetUser().Id;
            //賣家自己的賣家資訊，要讓買家畫面接收的
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            //取得買家資訊(以防他們沒有聊天紀錄過)
            var user = _repository.FirstOrDefault<User>(x => x.Id == receiverUserId);
            //發送者的畫面
            var sender = new ChatViewModel()
            {
                ReceiverImg = user.Photo,
                ReceiverName = user.Name,
                ReceiverId = user.Id,
                LastMessage = message,
                LastMessageTime = now.AddHours(8).ToString("HH:mm")
            };
            await Clients.Client(Context.ConnectionId).SendAsync("sellerSendMsgBox", sender);
            if (receiverUserId != 26)
            {
                //當使用者在線上時
                if (UserList.Any(x => x.UserId == receiverUserId))
                {
                    var reciver = new ChatViewModel()
                    {
                        ReceiverImg = seller.Icon,
                        ReceiverName = seller.ShopName,
                        ReceiverId = seller.Id,
                        LastMessage = message,
                        LastMessageTime = now.AddHours(8).ToString("HH:mm")
                    };
                    //取得買家的ConnectionId
                    var sendto = UserList.FirstOrDefault(x => x.UserId == receiverUserId).ConnectionId;
                    //接收訊息者(買家)的畫面
                    await Clients.Client(sendto).SendAsync("userUpdateMsgBox", reciver);
                    await Clients.Client(sendto).SendAsync("userLayout", reciver);
                }
            }
            else
            {
                string sendToGpt = "你是Hatsukoi電商的聊天機器人，請給賣家適當回答，且直接回答以下的問題:" + message;
                string apiKey = "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";
                string answer = string.Empty;
                var openai = new OpenAIAPI(apiKey);
                CompletionRequest completion = new CompletionRequest();
                completion.Prompt = sendToGpt;
                completion.MaxTokens = 3000;
                try
                {
                    var result = await openai.Completions.CreateCompletionAsync(completion);
                    if (result != null)
                    {
                        foreach (var item in result.Completions)
                        {
                            answer = item.Text;
                        }
                        await GptSendToSeller(seller.Id, answer);
                    }
                    else
                    {
                        await GptSendToSeller(seller.Id, "非常抱歉您的回答超出了我的範圍，您可以換個問題詢問我，像是: 幫我寫酷炫手表的介紹");
                    }
                }
                catch
                {
                    await GptSendToSeller(seller.Id, "非常抱歉您的回答超出了我的範圍，您可以換個問題詢問我，像是: 幫我寫酷炫手表的介紹");
                }
            }
            //將訊息記錄存進資料庫
            var newMessage = new Chatroom()
            {
                CreateTime = now,
                UserId = receiverUserId,
                SellerId = seller.Id,
                Message = message,
                MessageType = 2,
                IsRead = 2
            };
            _repository.Create(newMessage);
        }
        //GPT寄訊息給賣家
        public async Task GptSendToSeller(int receiverSellerId, string gptReply)
        {
            var message = gptReply.Substring(1);
            var now = DateTime.UtcNow;
            var userId = 26;
            var sellerUserId = _repository.FirstOrDefault<Seller>(x => x.Id == receiverSellerId).UserId;
            var user = _accountService.GetUserById(userId);
            //當使用者在線上時
            if (UserList.Any(x => x.UserId == sellerUserId))
            {
                var reciver = new ChatViewModel()
                {
                    ReceiverImg = user.Photo,
                    ReceiverName = user.Name,
                    ReceiverId = userId,
                    LastMessage = message,
                    LastMessageTime = now.AddHours(8).ToString("HH:mm")
                };
                //取得賣家的ConnectionId
                var sendto = UserList.FirstOrDefault(x => x.UserId == sellerUserId).ConnectionId;
                //接收訊息者(賣家)的畫面
                await Clients.Client(sendto).SendAsync("sellerUpdateMsgBox", reciver);
                await Clients.Client(sendto).SendAsync("sellerLayout", reciver);
            }
            //將訊息記錄存進資料庫
            var newMessage = new Chatroom()
            {
                CreateTime = now,
                UserId = userId,
                SellerId = receiverSellerId,
                Message = message,
                MessageType = 1,
                IsRead = 2
            };
            _repository.Create(newMessage);
        }
        //賣家發貨後寄送通知給User
        public async Task SendOrderMessage(string orderNumber)
        {
            var receiverUserId = _repository.FirstOrDefault<Order>(x => x.OrderNumber == orderNumber).UserId;
            var now = DateTime.UtcNow;
            //賣家自己的UserId
            var userId = _accountService.GetUser().Id;
            //賣家自己的賣家資訊，要讓買家畫面接收的
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            var msgText = $"您的訂單 {orderNumber} 包裹已經送達囉 ! 訂單沒問題後按下完成訂單即可評價商品 !!!";
            var msgTitle = "訂單已送達";
            var orderId = _repository.FirstOrDefault<Order>(x => x.OrderNumber == orderNumber).Id;
            var prodImg = _repository.FirstOrDefault<OrderDetail>(x => x.OrderId == orderId).ProductImg;
            //將訊息記錄存進資料庫
            var newMessage = new Notification()
            {
                SendTo = receiverUserId,
                SendTime = now,
                Text = msgText,
                Title = msgTitle,
                LinkTo = $"/User/OrderDetail?orderNum={orderNumber}",
                IsRead = 2,
                KindOfNotifi = 2,
                SendImg = prodImg
            };
            _repository.Create(newMessage);
            //var notiId = _repository.ListWhere<Notification>(x => x.SendTo == receiverUserId).OrderByDescending(x => x.SendTime).First().Id;
            //當使用者在線上時
            if (UserList.Any(x => x.UserId == receiverUserId))
            {
                //var reciver = new NotificationViewModel()
                //{
                //    Id = notiId,
                //    SendTime = now.ToString("F"),
                //    Text = msgText,
                //    Title = msgTitle,
                //    LinkTo = $"~/User/OrderDetail?orderNum={orderNumber}",
                //    SendImg = prodImg
                //};
                //取得買家的ConnectionId
                var sendto = UserList.FirstOrDefault(x => x.UserId == receiverUserId).ConnectionId;
                //接收訊息者(買家)的畫面
                //await Clients.Client(sendto).SendAsync("userUpdateNoti", reciver);
                await Clients.Client(sendto).SendAsync("userLayoutNoti", sendto);
            }
        }
        //賣家發送優惠券後通知給User
        public async Task SendCouponMessage(int receiverUserId)
        {
            var now = DateTime.UtcNow;
            //賣家自己的UserId
            var userId = _accountService.GetUser().Id;
            //賣家自己的賣家資訊，要讓買家畫面接收的
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            var msgText = $"您的已獲得 {seller.ShopName} 的優惠券 ! 優惠券已經送達您的優惠券清單 ! ";
            var msgTitle = "獲得優惠券";
            //將訊息記錄存進資料庫
            var newMessage = new Notification()
            {
                SendTo = receiverUserId,
                SendTime = now,
                Text = msgText,
                Title = msgTitle,
                LinkTo = $"/User/Coupon",
                IsRead = 2,
                KindOfNotifi = 1,
                SendImg = seller.Icon
            };
            _repository.Create(newMessage);
            //當使用者在線上時
            if (UserList.Any(x => x.UserId == receiverUserId))
            {
                //取得買家的ConnectionId
                var sendto = UserList.FirstOrDefault(x => x.UserId == receiverUserId).ConnectionId;
                //接收訊息者(買家)的畫面
                await Clients.Client(sendto).SendAsync("userLayoutNoti", sendto);
            }
        }
    }
}
