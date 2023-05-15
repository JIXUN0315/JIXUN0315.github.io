using Hatsukoi.Models.Dtos.Email;
using Hatsukoi.Models.Dtos.Seller;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Models.ViewModels.Seller;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using System.Drawing;
using NuGet.Protocol.Core.Types;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Common;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Service
{
    public class SellerService
    {
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IImageService _imageService;
        private readonly AccountService _accountService;
        public SellerService(IRepository repository, IEmailService emailService, IImageService imageService, AccountService accountService)
        {
            _repository = repository;
            _emailService = emailService;
            _imageService = imageService;
            _accountService = accountService;
        }
        //組裝SellerInfo的VM
        public InfoSettingViewModel GetSettingViewModel()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            InfoSettingViewModel output = new InfoSettingViewModel()
            {
                SellerId = seller.Id,
                City = seller.City,
                ApplyName = seller.ApplyName,
                IdNumber = seller.IdNumber,
                ApplyPhone = seller.ApplyPhone,
                Address = seller.Address,
                Email = seller.Email,
                ApplicantEnglishName = seller.ApplicantEnglishName ?? "",
                PostalCode = seller.PostalCode ?? "",
                SecondEmail = seller.SecondEmail ?? ""
            };
            return output;
        }
        //更新賣家資訊傳回來的資料到資料庫
        public void UpdateSellerInfo(SettingInfoDto dto)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            seller.ApplyPhone = dto.ApplyPhone;
            seller.PostalCode = dto.PostalCode;
            seller.ApplyName = dto.ApplyName;
            seller.ApplicantEnglishName = dto.ApplicantEnglishName;
            seller.Address = dto.Address;
            seller.City = dto.City;
            _repository.Update(seller);
        }
        //檢查Email是否有其他賣家使用
        public bool ExistSellerEmail(string email)
        {
            var one = _repository.Any<Seller>(x => x.Email == email);
            var two = _repository.Any<Seller>(x => x.SecondEmail == email);
            return one || two;
        }
        //祭出驗證第二信箱的信，並且將第二組信箱站存到資料庫
        public void SaveSecondEmailAndTime(string email)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            EmailDto sendemail = new EmailDto()
            {
                Email = email,
                EmailType = Common.HatsukoiEnum.EmailType.EditSellerSecondEmail,
                Id = seller.Id
            };
            _emailService.Send(sendemail);
            seller.StateSecondEmail = email;
            seller.StateSecondTime = DateTime.UtcNow;
            _repository.Update(seller);
        }
        //將第二組email更信進資料庫
        public void UpdateSecondEmail(Seller seller)
        {
            seller.SecondEmail = seller.StateSecondEmail;
            _repository.Update(seller);
        }
        //用SellerId取得Seller
        public Seller GetSellerBySellerId(int sellerId)
        {
            return _repository.FirstOrDefault<Seller>(x => x.Id == sellerId);
        }
        //組裝SellerIntroduction的VM
        public IntroductionViewModel GetIntroductionVM()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            var VM = new IntroductionViewModel()
            {
                Introduction = seller.Introduction,
                Logo = seller.Logo,
                ShopBannerSquare = seller.ShopBannerSquare,
                Icon = seller.Icon,
                ShopBannerRect = seller.ShopBannerRect,
                ShopName = seller.ShopName
            };
            return VM;
        }
        //將修改商店介紹的資料存進資料庫(包括圖片)
        public async Task EditShopDesign(IntroductionViewModel dto)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            seller.ShopName = dto.ShopName;
            seller.Introduction = dto.Introduction;
            var icon = await GetShopImg(dto.Icon, 100, 100);
            var logo = await GetShopImg(dto.Logo, 500, 150);
            var square = await GetShopImg(dto.ShopBannerSquare, 600, 600);
            var rect = await GetShopImg(dto.ShopBannerRect, 1000, 300);
            seller.Icon = icon;
            seller.Logo = logo;
            seller.ShopBannerSquare = square;
            seller.ShopBannerRect = rect;
            _repository.Update(seller);
        }
        //檢查圖片是否已經是網址，如果已經是就直接回傳，不適就轉成網址
        public async Task<string> GetShopImg(string img, int width, int height)
        {
            if (img.Contains("http"))
            {
                return img;
            }
            var output = await _imageService.GetImageUrl(img, width, height);
            return output;
        }
        //檢查是否商店名稱被使用過
        public bool ExistShopName(string shopName)
        {
            var list = _repository.ListWhere<Seller>(x => x.ShopName == shopName);
            if (list.Count == 0)
            {
                return false;
            }
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            if (seller.ShopName == list[0].ShopName)
            {
                return false;
            }
            return true;
        }
        //組裝渲染出賣家的休假設定VM
        public VacationViewModel GetVacationVM()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            bool isVacation = false;
            if (seller.VacationLastDay == null || seller.VacationLastDay < DateTime.Now)
            {
                isVacation = false;
                return new VacationViewModel()
                {
                    IsVacation = isVacation,
                    VacationFirstDay = "",
                    VacationLastDay = ""
                };
            }
            var first = (DateTime)seller.VacationFirstDay;
            var last = (DateTime)seller.VacationLastDay;
            return new VacationViewModel()
            {
                IsVacation = true,
                VacationFirstDay = first.ToString("yyyy-MM-dd"),
                VacationLastDay = last.ToString("yyyy-MM-dd")
            };
        }
        //關閉賣家休假
        public void CloseVaca()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            seller.VacationFirstDay = null;
            seller.VacationLastDay = null;
            _repository.Update(seller);
        }
        //開啟賣家休假
        public void OpenVaca(DateTime first, DateTime last)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            seller.VacationFirstDay = first;
            seller.VacationLastDay = last;
            _repository.Update(seller);
        }
        //組裝財務資訊的VM
        public FinanceViewModel GetFinanceVM()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            return new FinanceViewModel()
            {
                BankCode = seller.BankCode,
                BranchCode = seller.BranchCode,
                BankAccount = seller.BankAccount,
                BankAccountName = seller.BankAccountName,
                Email = seller.Email
            };
        }
        //用UserId取得Seller
        public int GetSellerIdByUserId(int userId)
        {
            var sellerId = _repository.ListWhere<Seller>(s => s.UserId == userId)
                .Select(s => s.Id).First();

            return sellerId;
        }
        //用PromoCode取得Coupon
        public Coupon GetCouponByPromoCode(string promoCode)
        {
             return _repository.FirstOrDefault<Coupon>(c => c.PromoCode == promoCode);
        }
        //取得賣家的所有優惠券
        public List<CouponViewModel> GetCouponBySellerId(int sellerId)
        {
            var couponList=_repository.ListWhere<Coupon>(x => x.SellerId == sellerId && x.DeleteStatus == (int?)DeleteStatus.None);
            return couponList.Select(c => new CouponViewModel
            { 
            PromoCode= c.PromoCode,
            Condition= (int)c.Condition,
            Discount= c.Discount * 10,
            StartTime=c.StartTime.ToString("yyyy/MM/dd"),
            EndTime=c.EndTime?.ToString("yyyy/MM/dd")
            }).ToList();
        }

        //新增優惠券
        public List<int> AddCoupon(CouponDto couponDto)
        {
            var userId = _accountService.GetUser().Id;
            var sellerId= _repository.FirstOrDefault<Seller>(x => x.UserId == userId).Id;
            // 轉換Dto為Coupon實體
            var coupon = new Coupon
            {
                SellerId = sellerId,
                PromoCode = couponDto.PromoCode,
                Condition = couponDto.Condition,
                Discount = couponDto.Discount,
                StartTime = couponDto.StartTime,
                EndTime = couponDto.EndTime,
                CreateTime = DateTime.UtcNow,
                DeleteStatus= (int?)DeleteStatus.None

            };
           
            _repository.Create(coupon);
            var userList = SendCoupon(couponDto.PromoCode, sellerId);
            return userList;
        }
        //更新優惠券
        public void UpdateCoupon(CouponDto couponDto)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId).Id;
            var couponList= _repository.FirstOrDefault<Coupon>(x => x.SellerId == seller);
            var coupon = _repository.FirstOrDefault<Coupon>(c => c.PromoCode == couponDto.PromoCode);
            coupon.Condition = couponDto.Condition;
            coupon.StartTime = couponDto.StartTime;
            coupon.EndTime = couponDto.EndTime;
            
        _repository.Update(coupon);

        }

        //刪除優惠券
        public void DeleteCoupon(string promoCode)
        {
            var coupon = _repository.FirstOrDefault<Coupon>(c => c.PromoCode == promoCode);
            coupon.DeleteStatus= (int?)DeleteStatus.Delete;
            _repository.Update(coupon);

            var couponId = coupon.Id;
            var couponListHasThis = _repository.ListWhere<CouponList>(c => c.CouponId == couponId);
            foreach (var c in couponListHasThis)
            {
                _repository.Delete(c);
            }

        }
        //寄送優惠券給有關注店家的用戶
        public List<int> SendCoupon(string promoCode,int sellerId)
        {
            var FavMyShopUser = _repository.ListWhere<FavShop>(s => s.SellerId == sellerId).Select(u => u.UserId).ToList();
            var getCouponId = _repository.FirstOrDefault<Coupon>(c => c.PromoCode == promoCode).Id;
            var couponListItems = FavMyShopUser.Select(userId => new CouponList
            {
                CouponId = getCouponId,
                UserId = userId,
                Status = (int)CouponStatus.usable, 
                CreateTime = DateTime.UtcNow 
            });
            foreach (var couponListItem in couponListItems)
            {
                _repository.Create(couponListItem);
            }
            var userList = couponListItems.Select(c => c.UserId).ToList();
            return userList;
        }
        //前端驗證檢查有沒有相同的PromoCode
        public bool CheckPromoCode(string promoCode)
        {
            return _repository.Any<Coupon>(x => x.PromoCode == promoCode);
        }

        //將修改的財務設定資料更新進資料庫
        public void UpdateFinance(FinanceDto dto)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            seller.BankCode = dto.BankCode;
            seller.BranchCode = dto.BranchCode;
            seller.BankAccount = dto.BankAccount;
            seller.BankAccountName = dto.BankAccountName;
            _repository.Update(seller);
        }
        //檢查財物的賣家驗證碼是否正確
        public bool CheckPassCode(string code)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _repository.FirstOrDefault<Seller>(x => x.UserId == userId);
            return seller.VerificationCode == code;
        }
    }
}
