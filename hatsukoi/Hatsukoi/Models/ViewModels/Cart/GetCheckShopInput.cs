namespace Hatsukoi.Models.ViewModels.Cart
{
    public class GetCheckShopInput
    {
        public int ShopId { get; set; }

        public int CouponId { get; set; }

        /// <summary>
        /// 已折抵金額
        /// </summary>
        public decimal DiscountAmount { get; set; }
    }
}
