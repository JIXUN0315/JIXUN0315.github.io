namespace Hatsukoi.Service.Interface
{
    public interface IImageService
    {
        Task<string> GetImageUrl(string base64Img, int width = 100, int height = 100);
        Task<List<string>> GetImageUrlList(List<string> base64Img, int width, int height) ;
        public string GetImageUrlByIFormFile(IFormFile myimg, int width = 100, int height = 100);
        public List<string> GetImageUrlListByIFormFile(List<IFormFile> myimg, int width, int height);
    }
}
