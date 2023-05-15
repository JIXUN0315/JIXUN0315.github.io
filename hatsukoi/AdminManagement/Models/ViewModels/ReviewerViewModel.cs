namespace AdminManagement.Models.ViewModels
{
    public class ReviewerViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ReviewTime { get; set; }
        public string Reviewstatus { get; set; }
        public string? FailReason { get; set; }
        public string ApplyName { get; set; }
        public string ApplyPhone { get; set; }
        public string ShopName { get; set; }
        public string Email { get; set; }
        public string ProductOrigin { get; set; }
        public string City { get; set; }
        public string SocialMedia { get; set; }
        public string Introduction { get; set; }
        public string CreateTime { get; set; }
        public string BrandName { get; set; }
        public List<string> ImgList { get; set; }
    }
}
