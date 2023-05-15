using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Hatsukoi.Service.Interface;

namespace Hatsukoi.Service
{
    public class ImageService : IImageService
    {
        public async Task<string> GetImageUrl(string base64Img, int width = 100, int height = 100)
        {
            string input = base64Img.Split(',')[1];
            var cloudinary = new Cloudinary(new Account("dwummfedl", "779832971883823", "_U3y6bJRe4apJh0XZEY2GRjljME"));
            var imageData = Convert.FromBase64String(input);

            ImageUploadResult uploadResult;

            using (var stream = new MemoryStream(imageData))
            {
                // 在這裡將 Stream 用於 Cloudinary 的圖片上傳
                // Upload
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("image.jpg", stream),
                    PublicId = "hatsukoi_item" + DateTime.UtcNow.ToString("s")
                };

                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            //Transformation
            cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(width).Height(height).Crop("fill"));
            return uploadResult.SecureUrl.ToString();
        }
        public async Task<List<string>> GetImageUrlList(List<string> base64Img, int width, int height)
        {
            var result = new List<string>();
            foreach(var img in base64Img)
            {
                result.Add(await GetImageUrl(img, width, height));
            }
            return result;
        }
        public string GetImageUrlByIFormFile(IFormFile myimg, int width = 100, int height = 100)
        {
            var cloudinary = new Cloudinary(new Account("dwummfedl", "779832971883823", "_U3y6bJRe4apJh0XZEY2GRjljME"));

            ImageUploadResult uploadResult;

            using (Stream stream = new MemoryStream())
            {
                stream.Flush();
                myimg.CopyToAsync(stream);
                stream.Position = 0;
                // 在這裡將 Stream 用於 Cloudinary 的圖片上傳
                // Upload
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("image.jpg", stream),
                    PublicId = "hatsukoi_item"
                };

                uploadResult = cloudinary.Upload(uploadParams);
            }

            //Transformation
            cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(width).Height(height).Crop("fill"));
            return uploadResult.SecureUrl.ToString();
        }
        public List<string> GetImageUrlListByIFormFile(List<IFormFile> myimg, int width, int height)
        {
            var result = new List<string>();
            foreach (var img in myimg)
            {
                result.Add(GetImageUrlByIFormFile(img, width, height));
            }
            return result;
        }
    }
}
