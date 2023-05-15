namespace Hatsukoi.Models.Dtos
{
    public class BeSellerDto
    {
        public string ApplyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ShopName { get; set; }
        public string Account { get; set; }
        public string SendFrom { get; set; }
        public string MadeIn { get; set; }
        public string SocialMedia { get; set; }
        public string Introduction { get; set; }
        public string IdNumber { get; set; }
        public string Componient { get; set; }
        public IEnumerable<string> ImgList { get; set; }
    }
}
