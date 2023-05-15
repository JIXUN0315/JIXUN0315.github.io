using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using AdminManagement.Models.ViewModels;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Repository.EntityModel;
using System.Drawing;
using static Dapper.SqlMapper;

namespace AdminManagement.Services
{
    public class BannerService
    {

        private readonly IRepository _repository;

        public BannerService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> GetImageUrl(string base64Img, int width = 100, int height = 100)
        {
            //base64編碼字串"data:image/png;base64, {imageData}"
            string input = base64Img.Split(',')[1];//取{imageData}
            //帳號名稱、API密鑰和API密碼
            var cloudinary = new Cloudinary(new Account("dwummfedl", "779832971883823", "_U3y6bJRe4apJh0XZEY2GRjljME"));
            //將 Base64 編碼的圖片轉換成 byte 陣列
            var imageData = Convert.FromBase64String(input);

            ImageUploadResult uploadResult;

            using (var stream = new MemoryStream(imageData))
            {
                // 在這裡將 Stream 用於 Cloudinary 的圖片上傳
                // Upload
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("image.jpg", stream),
                    PublicId = "hatsukoi_item" + DateTime.UtcNow.ToString()
                };

                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            //Transformation設定指定圖片的寬度、高度和裁剪模式
            cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(width).Height(height).Crop("fill"));
            return uploadResult.SecureUrl.ToString();
        }
        public async Task CreateBannerImage(string base64Img,int num)
        {
            var imageUrl = await GetImageUrl(base64Img, 400, 1000);

                // 將圖片URL存入資料庫
                var banner = new Banner
                {
                    ImgUrl = imageUrl,
                    Sort = num+1,
                    Status = 1
                };
                _repository.Create(banner);
        }

        public List<BannerViewModel> GetBannerImages()
        {
            var images = _repository.ListWhere<Banner>(b => b.Status==1).OrderBy(b => b.Sort).ToList();
            return images.Select(i=>new BannerViewModel
            {
                Id=i.Id,
                ImgUrl=i.ImgUrl,
                Sort= i.Sort,
                Status=i.Status
            }).ToList();

        }

        public void UpdateBannerImageSort(List<BannerViewModel> images)
        {
            foreach (var image in images)
            {
                var currentImage = _repository.FirstOrDefault<Banner>(b => b.Id == image.Id);
                if (currentImage != null)
                {
                    currentImage.Sort = image.Sort;
                    _repository.Update(currentImage);
                }
            }
        }

        public void DeleteBannerImage(int id)
        {
            var image =  _repository.FirstOrDefault<Banner>(b => b.Id == id);
            image.Sort = 0;
            image.Status = 0;

            if (image != null)
            {
                _repository.Update(image);
            }
        }

    }
}
