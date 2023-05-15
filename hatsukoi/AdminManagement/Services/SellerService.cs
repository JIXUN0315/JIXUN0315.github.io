using AdminManagement.Models.Dtos;
using AdminManagement.Models.ViewModels;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using System.Drawing;

namespace AdminManagement.Services
{
    public class SellerService
    {
        private readonly IRepository _repository;
        private readonly EmailService _emailRepository;

        public SellerService(IRepository repository, EmailService emailRepository)
        {
            _repository = repository;
            _emailRepository = emailRepository;
        }
        public List<ReviewerViewModel> GetReviewerViewModel()
        {
            var allReviewer = _repository.ListWhere<Reviewer>(x => true).OrderByDescending(x => x.LastEditTime).Select(x => new ReviewerViewModel
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = _repository.FirstOrDefault<User>(y => y.Id == x.UserId).Name,
                ReviewTime = x.ReviewTime == null ? "" : ((DateTime)x.ReviewTime).AddHours(8).ToString("yyyy/MM/dd-HH:mm"),
                Reviewstatus = x.Reviewstatus == 0 ? "未審核" : x.Reviewstatus == 1 ? "已駁回" : "已通過",
                FailReason = x.FailReason ?? "",
                ApplyName = x.ApplyName,
                ApplyPhone = x.ApplyPhone,
                ShopName = x.ShopName,
                BrandName = x.BrandName,
                Email = x.Email,
                ProductOrigin = x.ProductOrigin,
                City = x.City,
                SocialMedia = x.SocialMedia,
                Introduction = x.Introduction,
                CreateTime = x.CreateTime.AddHours(8).ToString("yyyy/MM/dd-HH:mm"),
                ImgList = _repository.ListWhere<ApplyImg>(y => y.ReviewerId == x.Id).Select(y => y.ImgUrl).ToList()
            }).ToList();
            return allReviewer;
        }
        public void ApplyReviewer(int reviewerId)
        {
            //修改reviewer資料
            var reviewer = _repository.FirstOrDefault<Reviewer>(x => x.Id == reviewerId);
            reviewer.Reviewstatus = 2;
            reviewer.ReviewTime = DateTime.UtcNow;
            reviewer.LastEditTime = DateTime.UtcNow;
            _repository.Update(reviewer);
            //寄信
            var account = Guid.NewGuid().ToString();
            var email = new EmailDto
            {
                Email = reviewer.Email,
                EmailType = 0,
                Account = account,
            };
            _emailRepository.Send(email);
            //修改User資料
            var user = _repository.FirstOrDefault<User>(x => x.Id == reviewer.UserId);
            user.StoreStatus = 3;
            _repository.Update(user);
            //新增Seller資料
            CreateSeller(reviewer, account);
        }
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
                Logo = @"https://res.cloudinary.com/dwummfedl/image/upload/v1681964784/hatsukoi_item2023-04-20T04:26:24.png",
                ModifiDate = DateTime.UtcNow,
                UserId = reviewer.UserId,
                Currency = 1,
                Address = reviewer.City,
                IdNumber = reviewer.IdNumber,
                ShopBannerSquare = @"https://res.cloudinary.com/dwummfedl/image/upload/v1681964857/hatsukoi_item2023-04-20T04:27:36.png",
                VerificationCode = verificationCode,
                Icon = @"https://res.cloudinary.com/dwummfedl/image/upload/v1681964857/hatsukoi_item2023-04-20T04:27:36.png",
                ShopBannerRect = @"https://res.cloudinary.com/dwummfedl/image/upload/v1681964784/hatsukoi_item2023-04-20T04:26:24.png",
                BrandName = reviewer.BrandName
            };
            _repository.Create(newSeller);
        }
        public void RejectReviewer(RejectReviewDto dto)
        {
            //修改reviewer資料
            var reviewer = _repository.FirstOrDefault<Reviewer>(x => x.Id == dto.ReviewerId);
            reviewer.Reviewstatus = 1;
            reviewer.ReviewTime = DateTime.UtcNow;
            reviewer.LastEditTime = DateTime.UtcNow;
            //寄信
            var account = Guid.NewGuid().ToString();
            var email = new EmailDto
            {
                Email = reviewer.Email,
                EmailType = 1,
                Reason = dto.Reason
            };
            _emailRepository.Send(email);
            //修改User資料
            var user = _repository.FirstOrDefault<User>(x => x.Id == reviewer.UserId);
            user.StoreStatus = 2;
            _repository.Update(user);
        }
        public List<SellerInfoViewModel> GetAllSeller()
        {
            var allShop = _repository.ListWhere<Seller>(x => true).Select(x => new SellerInfoViewModel
            {
                SellerId = x.Id,
                ApplyName = x.ApplyName,
                ApplyPhone = x.ApplyPhone,
                ShopName = x.ShopName,
                Email = x.Email,
                SocialMedia = x.SocialMedia,
                Introduction = x.Introduction,
                CreateDate = x.CreateDate.AddHours(8).ToString("yyyy/MM/dd"),
                Logo = x.Logo,
                Icon = x.Icon ,
                ShopBannerSquare = x.ShopBannerSquare ,
                ShopBannerRect = x.ShopBannerRect ,
                Status = _repository.FirstOrDefault<User>(y => y.Id == x.UserId).StoreStatus == 3 ? "正常" : "停權",
                Suspension = x.Story ?? ""
            }).ToList();
            return allShop;
        }
        public void SuspensionSeller(RejectReviewDto dto)
        {
            var seller = _repository.FirstOrDefault<Seller>(x => x.Id == dto.ReviewerId);
            //修改User資料
            var user = _repository.FirstOrDefault<User>(x => x.Id == seller.UserId);
            user.StoreStatus = 4;
            _repository.Update(user);
            //修改seller資料
            seller.Story = dto.Reason;
            _repository.Update(seller);
            //寄信
            var email = new EmailDto
            {
                Email = seller.Email,
                EmailType = 2,
                Reason = dto.Reason
            };
            _emailRepository.Send(email);
        }
        public void RestorationSuspension(int sellerId)
        {
            var seller = _repository.FirstOrDefault<Seller>(x => x.Id == sellerId);
            //修改User資料
            var user = _repository.FirstOrDefault<User>(x => x.Id == seller.UserId);
            user.StoreStatus = 3;
            _repository.Update(user);
            //修改seller資料
            seller.Story = string.Empty;
            _repository.Update(seller);
            //寄信
            var email = new EmailDto
            {
                Email = seller.Email,
                EmailType = 3
            };
            _emailRepository.Send(email);
        }
    }
}
