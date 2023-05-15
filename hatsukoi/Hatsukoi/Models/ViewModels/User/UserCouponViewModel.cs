namespace Hatsukoi.Models.ViewModels.User
{
    public class UserCouponViewModel
    {
        public string SellerName { get; set; }
        public decimal Condition { get; set; }
        public decimal Discount { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public string CouponNumber { get; set; }
        public string Img { get; set; }
    }
    public class UserCouponListViewModel
    {
        public string SellerName { get; set; }
        public decimal Condition { get; set; }
        public int Discount { get; set; }
        public DateTime EndTime { get; set; }
        public string CouponNumber { get; set; }
        public string Img { get; set; }
    }
}
