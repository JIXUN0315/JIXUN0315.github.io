using static Hatsukoi.Models.ViewModels.ProductViewModel;

namespace Hatsukoi.Models.ViewModels.Cart
{
    public class ShopCart
    {
        public int Id { get; set; }
        public string? ShopName { get; set; }
        public List<CartItem>? CartItems { get; set; }

        public int? CouponId { get; set; }

        public decimal? DiscountAmount { get; set;}


    }

    public class CartItem
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }
         
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? ItemImg { get; set; }
        public string? Description { get; set; }

        public int SpecID { get; set; }

        public string? SpecName { get; set;}

         

        //產品規格
        //public List<SpecList> SpecList { get; set; }
    }
    //public class SpecList
    //{

    //    public string SpecItem { get; set; }
    //    public decimal? UnitPrice { get; set; }
    //}
}
