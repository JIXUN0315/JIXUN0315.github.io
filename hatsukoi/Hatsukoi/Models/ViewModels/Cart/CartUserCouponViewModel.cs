using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Models.ViewModels.Cart
{
    public class CartUserCouponViewModel
    {
        public int CouponId { get; set; }
        public int sellerId { get; set; }
        public string SellerName { get; set; }
        public decimal Condition { get; set; }
        public decimal Discount { get; set; }
        public string DiscountStr { get; set; }
        public DateTime? EndTime { get; set; }
        public string? EndTimeStr { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeStr { get; set; }
        public string CouponNumber { get; set; }
        public string Img { get; set; }
    }
}
