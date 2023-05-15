using System.ComponentModel.DataAnnotations;

namespace Hatsukoi.Models.Dtos.Seller
{
    public class CouponDto
    {
        

        public string PromoCode { get; set; }

        public decimal Condition { get; set; }

        public decimal Discount { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
