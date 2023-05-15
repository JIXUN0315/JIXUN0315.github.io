using Hatsukoi.Models.ViewModels.ChatRoom;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;

namespace Hatsukoi.Service
{
    public class ChatRoomService
    {
        private readonly IRepository _repository;
        private readonly AccountService _accountService;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;

        public ChatRoomService(IRepository repository, AccountService accountService, IUserRepository userRepository,IImageService imageService)
        {
            _repository = repository;
            _accountService = accountService;
            _userRepository = userRepository;
            _imageService = imageService;
        }
        public List<ChatViewModel> GetUserChatRoomVM()
        {
            var userId = _accountService.GetUser().Id;
            var userTexts = _repository.ListWhere<Chatroom>(x => x.UserId == userId);
            var firstMsg = userTexts.GroupBy(x => x.SellerId).Select(x => x.OrderByDescending(y => y.CreateTime).First()).ToList();
            var MsgList = firstMsg.OrderByDescending(x => x.CreateTime).Select(x => new ChatViewModel
            {
                ReceiverName = _repository.FirstOrDefault<Seller>(y => y.Id == x.SellerId).ShopName,
                ReceiverId = x.SellerId,
                ReceiverImg = _repository.FirstOrDefault<Seller>(y => y.Id == x.SellerId).Icon,
                UnRead = userTexts.Where(y => y.SellerId == x.SellerId && y.MessageType == 2 && y.IsRead == 2).Count(),
                LastMessage = x.Message,
                LastMessageTime = x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") == DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd") ? x.CreateTime.AddHours(8).ToString("HH:mm") : x.CreateTime.AddHours(8).ToString("yyyy-MM-dd"),
            }).ToList();
            return MsgList;
        }
        public List<ChatViewModel> GetSellerChatRoomVM()
        {
            var userId = _accountService.GetUser().Id;
            var sellerId = _repository.FirstOrDefault<Seller>(x => x.UserId == userId).Id;
            var sellerTexts = _repository.ListWhere<Chatroom>(x => x.SellerId == sellerId);
            var firstMsg = sellerTexts.GroupBy(x => x.UserId).Select(x => x.OrderByDescending(y => y.CreateTime).First()).ToList();
            var MsgList = firstMsg.OrderByDescending(x => x.CreateTime).Select(x => new ChatViewModel
            {
                ReceiverName = _repository.FirstOrDefault<User>(y => y.Id == x.UserId).Name,
                ReceiverId = x.UserId,
                ReceiverImg = _repository.FirstOrDefault<User>(y => y.Id == x.UserId).Photo,
                UnRead = sellerTexts.Where(y => y.UserId == x.UserId && y.MessageType == 1 && y.IsRead == 2).Count(),
                LastMessage = x.Message,
                LastMessageTime = x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") == DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd") ? x.CreateTime.AddHours(8).ToString("HH:mm") : x.CreateTime.AddHours(8).ToString("yyyy-MM-dd")
            }).ToList();
            return MsgList;
        }
        public Receivers GetSellerChatVM()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            var isCahtWithGpt = _repository.Any<Chatroom>(x => x.SellerId == seller.Id && x.UserId == 26);
            if (!isCahtWithGpt)
            {
                var fistMsg = new Chatroom
                {
                    UserId = 26,
                    SellerId = seller.Id,
                    Message = "你好我是你專屬的設計館小助手，我可以給你設計館相關的幫助，例如: 該如何賣出好商品、我該怎麼寫商品介紹、該如何介紹我的設計館。 有任何問題歡迎詢問我喔!",
                    CreateTime = DateTime.UtcNow,
                    MessageType = 1,
                    IsRead = 2
                };
                _repository.Create(fistMsg);
            }
            var VM = new Receivers()
            {
                Img = seller.Icon,
                Name = seller.ShopName
            };
            return VM;
        }
        public Receivers GetUserChatVM()
        {
            var userId = _accountService.GetUser().Id;
            var isCahtWithGpt = _repository.Any<Chatroom>(x => x.UserId == userId && x.SellerId == 27);
            if (!isCahtWithGpt)
            {
                var fistMsg = new Chatroom
                {
                    UserId = userId,
                    SellerId = 27,
                    Message = "你好我是你專屬的聊天機器人，有任何問題歡迎詢問我喔!",
                    CreateTime = DateTime.UtcNow,
                    MessageType = 2,
                    IsRead = 2
                };
                _repository.Create(fistMsg);
            }
            var user = _repository.FirstOrDefault<User>(x => x.Id == userId);
            var VM = new Receivers()
            {
                Img = user.Photo,
                Name = user.Name
            };
            return VM;
        }
        public MessageDetailViewModel GetMessageDetailViewModels(int receiveId)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.Id == receiveId);
            var allMsg = _repository.ListWhere<Chatroom>(x => x.UserId == userId && x.SellerId == receiveId).OrderByDescending(x => x.CreateTime).Take(10).Reverse().ToList();
            var messages = allMsg.Select(x => new MessageDetail
            {
                MsgId = x.Id,
                MsgType = x.MessageType,
                Message = x.Message,
                SendTime = x.CreateTime.AddHours(8).ToString("HH:mm"),// == DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd") ? x.CreateTime.AddHours(8).ToString("HH:mm") : x.CreateTime.AddHours(8).ToString("F"),
                ChangeTime = allMsg.IndexOf(x) == 0 ? x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") : allMsg[allMsg.IndexOf(x) - 1].CreateTime.AddHours(8).ToString("yyyy-MM-dd") == x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") ? "" : x.CreateTime.AddHours(8).ToString("yyyy-MM-dd")
            }).ToList();
            var read = _repository.ListWhere<Chatroom>(x => x.UserId == userId && x.SellerId == receiveId && x.MessageType == 2 && x.IsRead == 2);
            _userRepository.UpdateList(read);
            var VM = new MessageDetailViewModel()
            {
                Name = seller.ShopName,
                Img = seller.Icon,
                Messages = messages
            };
            return VM;
        }
        public MessageDetailViewModel GetSellerMessageDetailViewModels(int userId)
        {
            var user = _repository.FirstOrDefault<User>(x => x.Id == userId);
            var sellerUserId = _accountService.GetUser().Id;
            var sellerId = _repository.FirstOrDefault<Seller>(x => x.UserId == sellerUserId).Id;
            var allMsg = _repository.ListWhere<Chatroom>(x => x.UserId == userId && x.SellerId == sellerId).OrderByDescending(x => x.CreateTime).Take(10).Reverse().ToList();
            var messages = allMsg.Select(x => new MessageDetail
            {
                MsgId = x.Id,
                MsgType = x.MessageType,
                Message = x.Message,
                SendTime = x.CreateTime.AddHours(8).ToString("HH:mm"),// == DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd") ? x.CreateTime.AddHours(8).ToString("HH:mm") : x.CreateTime.AddHours(8).ToString("F"),
                ChangeTime = allMsg.IndexOf(x) == 0 ? x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") : allMsg[allMsg.IndexOf(x) - 1].CreateTime.AddHours(8).ToString("yyyy-MM-dd") == x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") ? "" : x.CreateTime.AddHours(8).ToString("yyyy-MM-dd")
            }).ToList();
            var read = _repository.ListWhere<Chatroom>(x => x.UserId == userId && x.SellerId == sellerId && x.MessageType == 1 && x.IsRead == 2);
            _userRepository.UpdateList(read);
            var VM = new MessageDetailViewModel()
            {
                Name = user.Name,
                Img = user.Photo,
                Messages = messages
            };
            return VM;
        }
        public StarMessage GetStar(string sellerId, string msg)
        {
            var Id = int.Parse(sellerId);
            var seller = _repository.FirstOrDefault<Seller>(x => x.Id == Id);
            var output = new StarMessage()
            {
                ReciverId = Id,
                Name = seller.ShopName,
                Img = seller.Icon,
                Messages = msg
            };
            return output;
        }
        public StarMessage GetSellerStar(string userId, string msg)
        {
            var Id = int.Parse(userId);
            var user = _repository.FirstOrDefault<User>(x => x.Id == Id);
            var output = new StarMessage()
            {
                ReciverId = Id,
                Name = user.Name,
                Img = user.Photo,
                Messages = msg
            };
            return output;
        }
        public bool UserIsChatMyself(int id)
        {
            var seller = _repository.FirstOrDefault<Seller>(x => x.Id== id);
            var userId = _accountService.GetUser().Id;
            return seller.UserId == userId;
        }
        public bool SellerIsChatMyself(int id)
        {
            var userId = _accountService.GetUser().Id;
            return id == userId;
        }
        public MessageDetailViewModel UserGet_5_OlderMsg(int nowMsgCount, int chatSellerId)
        {
            var userId = _accountService.GetUser().Id;
            //新到舊
            var allMsg = _repository.ListWhere<Chatroom>(x => x.UserId == userId && x.SellerId == chatSellerId).OrderByDescending(x => x.CreateTime).Skip(nowMsgCount).Take(5).Reverse().ToList();
            var output = allMsg.Select(x => new MessageDetail()
            {
                MsgId = x.Id,
                MsgType = x.MessageType,
                Message = x.Message,
                SendTime = x.CreateTime.AddHours(8).ToString("HH:mm"),// == DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd") ? x.CreateTime.AddHours(8).ToString("HH:mm") : x.CreateTime.AddHours(8).ToString("F"),
                ChangeTime = allMsg.IndexOf(x) == 0 ? x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") : allMsg[allMsg.IndexOf(x) - 1].CreateTime.AddHours(8).ToString("yyyy-MM-dd") == x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") ? "" : x.CreateTime.AddHours(8).ToString("yyyy-MM-dd")
            }).ToList();
            var msg5 = new MessageDetailViewModel()
            {
                Messages = output
            };
            return msg5;
        }
        public MessageDetailViewModel SellserGet_5_OlderMsg(int nowMsgCount, int chatUserId)
        {
            var userId = _accountService.GetUser().Id;
            var sellerId = _repository.FirstOrDefault<Seller>(x => x.UserId == userId).Id;
            //新到舊
            var allMsg = _repository.ListWhere<Chatroom>(x => x.UserId == chatUserId && x.SellerId == sellerId).OrderByDescending(x => x.CreateTime).Skip(nowMsgCount).Take(5).Reverse().ToList();
            var output = allMsg.Select(x => new MessageDetail()
            {
                MsgId = x.Id,
                MsgType = x.MessageType,
                Message = x.Message,
                SendTime = x.CreateTime.AddHours(8).ToString("HH:mm"),// == DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd") ? x.CreateTime.AddHours(8).ToString("HH:mm") : x.CreateTime.AddHours(8).ToString("F"),
                ChangeTime = allMsg.IndexOf(x) == 0 ? x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") : allMsg[allMsg.IndexOf(x) - 1].CreateTime.AddHours(8).ToString("yyyy-MM-dd") == x.CreateTime.AddHours(8).ToString("yyyy-MM-dd") ? "" : x.CreateTime.AddHours(8).ToString("yyyy-MM-dd")
            }).ToList();
            var msg5 = new MessageDetailViewModel()
            {
                Messages = output
            };
            return msg5;
        }
        public void UserRaedMsg(int sellerId)
        {
            var aa = _repository.FirstOrDefault<Seller>(x => x.Id == sellerId);
            var userId = _accountService.GetUser().Id;
            var msg = _repository.FirstOrDefault<Chatroom>(x => x.UserId == userId && x.SellerId == sellerId && x.MessageType == 2 && x.IsRead == 2);
            if (msg != null)
            {
                msg.IsRead = 1;
                _repository.Update(msg);
            }
        }
        public void SellerRaedMsg(int userId)
        {
            var myUserId = _accountService.GetUser().Id;
            var sellerId = _repository.FirstOrDefault<Seller>(x => x.UserId == myUserId).Id;
            var msg = _repository.FirstOrDefault<Chatroom>(x => x.UserId == userId && x.SellerId == sellerId && x.MessageType == 1 && x.IsRead == 2);
            if(msg!= null)
            {
                msg.IsRead = 1;
                _repository.Update(msg);
            }
        }
        public int GetSellerUnRead()
        {
            var userId = _accountService.GetUser().Id;
            var sellerId = _repository.FirstOrDefault<Seller>(x => x.UserId == userId).Id;
            var msg = _repository.ListWhere<Chatroom>(x => x.SellerId == sellerId && x.MessageType == 1 && x.IsRead == 2).Count;
            return msg;
        }
        public async Task<string> GetImgUrl(string img)
        {
            var url = await _imageService.GetImageUrl(img);
            var imgTag = $"<div class='w-100'><img class='send-pic' src='{url}' /></div>";
            return imgTag;
        }
    }
}
