namespace Hatsukoi.Models.ViewModels.Seller
{
    public class CouponViewModel
    {
      
        public decimal Discount { get; set; }

        public decimal Condition { get; set; }

        public string CreateTime { get; set; }

        public string StartTime { get; set; }

        public string? EndTime { get; set; }

        public string PromoCode { get; set; } 
    }
}
