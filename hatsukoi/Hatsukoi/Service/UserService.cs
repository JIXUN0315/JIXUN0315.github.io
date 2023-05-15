using CloudinaryDotNet;
using Hatsukoi.Common;
using Hatsukoi.Models.Dtos;
using Hatsukoi.Models.Dtos.Email;
using Hatsukoi.Models.Dtos.User;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Models.ViewModels.User;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using MailKit.Search;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using static Hatsukoi.Common.HatsukoiEnum;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Hatsukoi.Service
{
    public class UserService
    {
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IImageService _imageService;
        private readonly IAccountService _accountService;
        public UserService(IRepository repository, IEmailService emailService, IImageService imageService, IAccountService accountService)
        {
            _repository = repository;
            _emailService = emailService;
            _imageService = imageService;
            _accountService = accountService;
        }
        //申請成為賣家前端船回來的資料轉成資料庫Reviewer要的資料
        public ApplySellerDto GetApplySeller(BeSellerDto dto)
        {
            return new ApplySellerDto()
            {
                ApplyName = dto.ApplyName,
                Phone = dto.Phone,
                Email = dto.Email,
                SendFrom = dto.SendFrom,
                ShopName = dto.ShopName,
                Account = dto.Account,
                MadeIn = dto.MadeIn,
                SocialMedia = dto.SocialMedia,
                Introduction = dto.Introduction,
                IdNumber= dto.IdNumber,
                Componient = dto.Componient
            };
        }
        //資料庫新增Reviewer的資料
        public void CreateReviewer(ApplySellerDto dto, int userId)
        {
            var newReviewer = new Reviewer()
            {
                UserId = userId,
                Reviewstatus = (int)HatsukoiEnum.Reviewstatus.NotReview,
                ApplyName = dto.ApplyName,
                ApplyPhone = dto.Phone,
                ShopName= dto.ShopName,
                Email = dto.Email,
                ProductOrigin = dto.MadeIn,
                City = dto.SendFrom,
                SocialMedia= dto.SocialMedia,
                Introduction= dto.Introduction,
                CreateTime = DateTime.UtcNow,
                IdNumber = dto.IdNumber,
                BrandName = dto.Componient,
                LastEditTime = DateTime.UtcNow
            };
            _repository.Create(newReviewer);
        }
        //檢查商家名稱是否被使用過
        public async Task<bool> ExistShopName(string name)
        {
            return await _repository.AnyAsync<Seller>(x => x.ShopName == name);
        }
        //檢查email是否被使用過
        public async Task<bool> ExistSellerEmail(string email)
        {
            return await _repository.AnyAsync<Seller>(x => x.Email == email);
        }
        //資料庫新增Seller的資料
        public void CreateSeller(Reviewer reviewer, string verificationCode)
        {
            var newSeller = new Seller()
            {
                ApplyName = reviewer.ApplyName,
                ApplyPhone = reviewer.ApplyPhone,
                ShopName = reviewer.ShopName,
                Email = reviewer.Email,
                City = reviewer.City,
                ProductOrigin = reviewer.ProductOrigin,
                SocialMedia = reviewer.SocialMedia,
                Introduction = reviewer.Introduction,
                CreateDate = DateTime.UtcNow,
                Logo = @"https://res.cloudinary.com/dwummfedl/image/upload/v1679974131/hatsukoi_item.png",
                ModifiDate = DateTime.UtcNow,
                UserId = reviewer.UserId,
                Currency = 1,
                Address = reviewer.City,
                IdNumber = reviewer.IdNumber,
                ShopBannerSquare = @"https://res.cloudinary.com/dwummfedl/image/upload/v1679974155/hatsukoi_item.png",
                VerificationCode = verificationCode,
                Icon = @"https://res.cloudinary.com/dwummfedl/image/upload/v1679974155/hatsukoi_item.png",
                ShopBannerRect = @"https://res.cloudinary.com/dwummfedl/image/upload/v1679974131/hatsukoi_item.png",
                BrandName = reviewer.BrandName
            };
            _repository.Create(newSeller);
        }
        //創建隨機亂碼給成為商家的人
        public string SetSellerVerificationCode()
        {
            return Guid.NewGuid().ToString();
        }
        //用UserId取得最近一筆的Reviewer資料
        public Reviewer GetReviewerByUserId(int id)
        {
            var reviewerApply = _repository.ListWhere<Reviewer>(x => x.UserId == id).OrderByDescending(x => x.CreateTime).ToList();
            return reviewerApply[0];
        }
        //成為賣家後將Reviewer的狀態欄位改成成功
        public void ReviewerStatusChangeSuccess(Reviewer reviewer)
        {
            reviewer.Reviewstatus = (int)HatsukoiEnum.Reviewstatus.Success;
            _repository.Update(reviewer);
        }
        //申請成為賣家或者是成為賣家都需要改變User的商家狀態
        public async void ChangeUserStoreStatus(int userId, HatsukoiEnum.StoreStatus status)
        {
            var user = await _repository.FirstOrDefaultAsync<User>(x => x.Id == userId);
            user.StoreStatus = (int)status;
            _repository.Update(user);
        }
        //存進圖片轉換網址後存進資料庫
        public async Task SaveReviewerImages(List<string> Images, int ReviewerId)
        {
            var imageList = await _imageService.GetImageUrlList(Images, 100, 100);
            foreach (var image in imageList)
            {
                var reviewerImage = new ApplyImg()
                {
                    ReviewerId= ReviewerId,
                    ImgUrl = image
                };
                _repository.Create(reviewerImage);
            }
        }
        //寄信申請成為賣家流程
        public async Task SendApplyEmail(BeSellerDto dto, int userId)
        {
            var apply = GetApplySeller(dto);
            CreateReviewer(apply, userId);
            //存進圖片
            var imgList = dto.ImgList.ToList();
            var reviewer = GetReviewerByUserId(userId);
            //將圖片資料存進ReviewerImg資料表
            await SaveReviewerImages(imgList, reviewer.Id);

            ChangeUserStoreStatus(userId, HatsukoiEnum.StoreStatus.UnderReview);
            EmailDto email = new EmailDto()
            {
                Email = dto.Email,
                EmailType = Common.HatsukoiEnum.EmailType.ApplySeller,
                Id = userId
            };
            _emailService.Send(email);
        }
        //寄信成功成為賣家流程
        public void SendSuccessEmail(BeSellerDto dto, int userId)
        {
            var identity = SetSellerVerificationCode();
            var reviewer = GetReviewerByUserId(userId);
            CreateSeller(reviewer, identity);
            ReviewerStatusChangeSuccess(reviewer);
            ChangeUserStoreStatus(userId, HatsukoiEnum.StoreStatus.Pass);
            EmailDto successEmail = new EmailDto()
            {
                Email = dto.Email,
                EmailType = Common.HatsukoiEnum.EmailType.BeSellerSuccess,
                Id = userId,
                Account = identity
            };
            _emailService.Send(successEmail);
        }
        //封裝好User的Order畫面的Model並且依照訂單狀態產生
        public List<UserOrderViewModel> GetUserStatusOrders(HatsukoiEnum.OrderStatus orderStatus, int userId)
        {
            var orders = _repository.ListWhere<Order>(x => x.Status == (int)orderStatus && x.UserId == userId).OrderByDescending(x => x.StatusFinishTime).ThenByDescending(x => x.StatusSendTime).ThenByDescending(x => x.CreateTime).ThenByDescending(x => x.StatusCancelTime);
            var VM = orders.Select(x => new UserOrderViewModel
            {
                OrderId = x.Id,
                ShopName = _repository.FirstOrDefault<OrderDetail>(y => y.OrderId == x.Id).SellerName,
                OrderStatus = (int)orderStatus,
                RecipientAddress = x.RecipientAddress,
                RecipientName = x.RecipientName,
                RecipientPhone = x.RecipientPhone,
                OrderTime = x.CreateTime,
                TotalPrice = x.TotalPrice,
                FirstImg = _repository.FirstOrDefault<OrderDetail>(y => x.Id == y.OrderId).ProductImg,
                CouponCount = _repository.ListWhere<OrderDetail>(y => y.OrderId == x.Id).Sum(y => y.UnitPrice*y.Quantity) - x.TotalPrice,
                OrderNumber = x.OrderNumber,
                RecipientCity = x.RecipientCity,
                SellerId = x.SellerId
            }).ToList();
            return VM;
        }
        //用Order的訂單編號取得Order
        private Order GetOrderByOrderNumber(string orderNum)
        {
            return _repository.FirstOrDefault<Order>(x => x.OrderNumber== orderNum);
        }
        //組裝渲染在User的OrderDetail畫面的ViewModel
        public UserOrderDetailViewModel GetUserOrder(string orderNum)
        {
            var orderDetail = _repository.ListWhere<OrderDetail>(x => true);
            var order = GetOrderByOrderNumber(orderNum);
            var productSpecification = _repository.ListWhere<ProductSpecification>(x => true);
            var odList = orderDetail.Where(x => x.OrderId == order.Id).Select(x => new OrderDetailListViewModel
            {
                ProductID = productSpecification.First(y => y.Id == x.ProductSpecificationId).ProductId,
                ProductName = x.ProductName,
                FirstSepcItem = x.FirstSepcItem,
                SecondSepcItem = x.SecondSepcItem,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                ProductImg = x.ProductImg
            }).ToList();
            var existEvaluate = order.Evaluate != null;
            var shopName = orderDetail.First(y => y.OrderId == order.Id).SellerName;
            var FirstImg = orderDetail.First(y => order.Id == y.OrderId).ProductImg;
            var CouponCount = order.TotalPrice - orderDetail.Where(y => y.OrderId == order.Id).Sum(y => y.UnitPrice * y.Quantity);
            var output = new UserOrderDetailViewModel()
            {
                OrderId = order.Id,
                ShopName = shopName,
                OrderStatus = (int)order.Status,
                RecipientAddress = order.RecipientAddress,
                RecipientName = order.RecipientName,
                RecipientPhone = order.RecipientPhone,
                OrderTime = order.CreateTime,
                TotalPrice = order.TotalPrice,
                FirstImg = FirstImg,
                CouponCount = CouponCount,
                OrderNumber = orderNum,
                RecipientCity = order.RecipientCity,
                SellerId = order.SellerId,
                ExistEvaluate = existEvaluate,
                OrderDetailItems = odList
            };
            return output;
        }
        //按下完成訂單的按鈕對資料庫訂單的狀態修改為已完成
        public void FinishOrderByOrderNumber(string orderNum)
        {
            var order = GetOrderByOrderNumber(orderNum);
            order.Status = (int)HatsukoiEnum.OrderStatus.Completed;
            order.StatusFinishTime = DateTime.UtcNow;
            _repository.Update(order);
        }
        //按下評價訂單後將評價的資料存進Order資料庫
        public void SetEvaluate(EvaluateDto dto)
        {
            var order = GetOrderByOrderNumber(dto.orderNum);
            order.Evaluate = dto.Evaluate;
            order.EvaluateText = dto.EvaluateText;
            order.EvaluateDate = DateTime.UtcNow;
            _repository.Update(order);
        }
        //進入User基本資料畫面組裝需要渲染化的的ViewModel
        //計算升級成為下一等還需要消費多少金額
        public MemberLevelDto GetYearLevel(int userId, HatsukoiEnum.WhichYear year)
        {
            var thisYearOrderPrice = GetThisYearPrice(userId);
            var lastYearOrderPrice = GetLastYearPrice(userId);
            MemberLevelDto Member;
            if (year == WhichYear.ThisYear)
            {
                Member = GetMemberLevel(lastYearOrderPrice + thisYearOrderPrice);
                if(Member.price == 0)
                {
                    return new MemberLevelDto
                    {
                        Level = "白銀會員",
                        price = 1000,
                    };
                }
                return new MemberLevelDto
                {
                    Level = Member.Level,
                    price = Member.price - lastYearOrderPrice - thisYearOrderPrice,
                };
            }
            Member = GetMemberLevel(thisYearOrderPrice);
            if (Member.price == 0)
            {
                return new MemberLevelDto
                {
                    Level = "白銀會員",
                    price = 1000,
                };
            }
            return new MemberLevelDto
            {
                Level = Member.Level,
                price = Member.price - thisYearOrderPrice,
            };
        }
        //取得今年的累積金額
        public decimal GetThisYearPrice(int userId)
        {
            var allorder = _repository.ListWhere<Order>(x => x.UserId == userId);
            var thisYear = DateTime.UtcNow.Year;
            var output = allorder.Where(x => x.PayTime != null && x.PayTime.Value.Year == thisYear).Sum(x => x.TotalPrice);
            return output;
        }
        public decimal GetLastYearPrice(int userId)
        {
            var allorder = _repository.ListWhere<Order>(x => x.UserId == userId);
            var thisYear = DateTime.UtcNow.Year;
            var lastYear = thisYear - 1;
            return allorder.Where(x => x.PayTime != null && x.PayTime.Value.Year == lastYear).Sum(x => x.TotalPrice);
        }
        //取得今年當前等級，並且將等級更新進資料庫
        public async Task<string> GetThisYearLevel(decimal thisPrice)
        {
            List<MemberLevelDto> memberLevels = new List<MemberLevelDto>()
            {
                new MemberLevelDto(){ Level = "尊爵會員", price= 10000 },
                new MemberLevelDto(){ Level = "鑽石會員", price= 5000 },
                new MemberLevelDto(){ Level = "黃金會員", price= 3000 },
                new MemberLevelDto(){ Level = "白銀會員", price= 1000 },
                new MemberLevelDto(){ Level = "品藍會員", price= 0 },
            };
            var thisLevel = memberLevels.First(x => x.price <= thisPrice);
            var hatsukoilevel = 4 - memberLevels.IndexOf(thisLevel);
            var userId = _accountService.GetUser().Id;
            var user = await _repository.FirstOrDefaultAsync<User>(x => x.Id == userId);
            user.MemberLevel = hatsukoilevel;
            _repository.Update(user);
            return thisLevel.Level;
        }
        //取得下一等級資訊
        public MemberLevelDto GetMemberLevel(decimal thisPrice)
        {
            List<MemberLevelDto> memberLevels = new List<MemberLevelDto>()
            {
                new MemberLevelDto(){ Level = "品藍會員", price= 0 },
                new MemberLevelDto(){ Level = "白銀會員", price= 1000 },
                new MemberLevelDto(){ Level = "黃金會員", price= 3000 },
                new MemberLevelDto(){ Level = "鑽石會員", price= 5000 },
                new MemberLevelDto(){ Level = "尊爵會員", price= 10000 },
                new MemberLevelDto(){ Level = "", price= decimal.MaxValue },
            };
            return memberLevels.First(x => x.price >= thisPrice);
        }
        //取得User的Id
        public User GetUserByUserId(int id)
        {
            return _repository.FirstOrDefault<User>(x => x.Id == id);
        }
        //產出User的Coupon View Model
        public List<UserCouponViewModel> GetUserCouponViewModel(int userId)
        {
            string query = $@"
                    SELECT
	                    s.ShopName AS SellerName,
	                    c.Condition AS Condition,
	                    c.Discount AS Discount,
                        c.StartTime AS StartTime,
	                    c.EndTime AS EndTime,
	                    c.PromoCode AS CouponNumber,
	                    s.ShopBannerSquare AS Img
                    FROM CouponList cl
                    INNER JOIN Coupon c ON c.Id = cl.CouponId
                    INNER JOIN Seller s ON c.SellerId = s.Id
                    WHERE cl.UserId = ${userId}
                ";
            var couponList = _repository.GetSQLQuery<UserCouponViewModel>(query);
            //var conn = _config.GetConnectionString("HatsukoiContext");
            //List<UserCouponViewModel> couponList;
            //using (var context = new System.Data.Entity.DbContext(conn))
            //{
            //    couponList = context.Database.SqlQuery<UserCouponViewModel>(
            //        $@"
            //        SELECT
	           //         s.ShopName AS SellerName,
	           //         c.Condition AS Condition,
	           //         c.Discount AS Discount,
	           //         c.EndTime AS EndTime,
	           //         c.PromoCode AS CouponNumber,
	           //         s.ShopBannerSquare AS Img
            //        FROM CouponList cl
            //        INNER JOIN Coupon c ON c.Id = cl.CouponId
            //        INNER JOIN Seller s ON c.SellerId = s.Id
            //        WHERE cl.UserId = ${userId}
            //    ").ToList();
            //}
            var list = couponList.Select(x => new UserCouponViewModel()
            {
                SellerName = x.SellerName,
                Condition = x.Condition,
                Discount = x.Discount*10,
                EndTime = x.EndTime,
                StartTime = x.StartTime,
                CouponNumber = x.CouponNumber,
                Img = x.Img
            }).ToList();
            return list;
        }
        //產出User的設定ViewModel
        public UserSettingViewModel GetUserSettingViewModel(int id)
        {
            var user = _repository.FirstOrDefault<User>(x => x.Id == id);
            return new UserSettingViewModel()
            {
                UserName = user.Name,
                Gender = user.Gender,
                Birthday = user.BirthDate,
                Img = user.Photo,
                Email = user.Email
            };
        }
        //將傳進來的圖片資料做檢查是不是網址，如果是就直接回傳，如果不是就轉換成網址
        public async Task<string> GetUserImg(string img)
        {
            if (img.Contains("http"))
            {
                return img;
            }
            return await _imageService.GetImageUrl(img, 100, 100);
        }
        //將Info傳回來的資料更新進資料庫
        public void EditUser(User user)
        {
            _repository.Update(user);
        }
        //組裝Email通知畫面的ViewModel
        public EmailNotifiViewModel GetEmailNotifiVM()
        {
            var userId = _accountService.GetUser().Id;
            var user = GetUserByUserId(userId);
            var VM = new EmailNotifiViewModel()
            {
                Order = user.IsEmailOrder ? "checked":null,
                Hatsukoi = user.IsEmailHatsukoi ? "checked" : null,
                Activity = user.IsEmailActivity ? "checked" : null,
                Week = user.IsEmailWeek ? "checked" : null,
                Member = user.IsEmailMember ? "checked" : null,
                Follow = user.IsEmailFollow ? "checked" : null,
                Focus = user.IsEmailFocus ? "checked" : null
            };
            return VM;
        }
        //將Email通知的資料更新進資料庫
        public void UpdateEmailNotifi(EmailNotifiDto dto)
        {
            var userId = _accountService.GetUser().Id;
            var user = GetUserByUserId(userId);
            user.IsEmailActivity = dto.Activity;
            user.IsEmailWeek = dto.Week;
            user.IsEmailMember = dto.Member;
            user.IsEmailFollow = dto.Follow;
            user.IsEmailFocus = dto.Focus;
            user.IsEmailOrder = dto.Order;
            user.IsEmailHatsukoi = dto.Hatsukoi;
            _repository.Update(user);
        }
        //檢查帳號是否有綁定google登入，如果有的話不給重設email
        public bool ExistGoogle()
        {
            var userId = _accountService.GetUser().Id;
            var user = GetUserByUserId(userId);
            if(user.Identifier == null)
            {
                return false;
            }
            return true;
        }
        //寄出修改email的信
        public void SendChangeEmail(string newEmail)
        {
            var userId = _accountService.GetUser().Id;
            EmailDto email = new EmailDto()
            {
                Email = newEmail,
                EmailType = Common.HatsukoiEnum.EmailType.ChangeEmail,
                Id = userId
            };
            _emailService.Send(email);
        }
        //更新新的Email近資料庫準備更新Email欄位
        public void UpdateNewEmail(string email)
        {
            var userId = _accountService.GetUser().Id;
            var user = _accountService.GetUserById(userId);
            user.NewEmail = email;
            user.ChangeEmailTime = DateTime.UtcNow;
            _repository.Update(user);
        }
        //將新的Email更新成使用者的Email，並且刪除新Email欄位
        public void UpdateEmailFromNewEmail(User user)
        {
            user.Email = user.NewEmail;
            user.NewEmail = null;
            _repository.Update(user);
        }
    }
}
