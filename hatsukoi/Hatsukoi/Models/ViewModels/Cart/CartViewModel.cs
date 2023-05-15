namespace Hatsukoi.Models.ViewModels.Cart
{
    public class CartViewModel
    {

        // 購物車Id
        public int Id { get; set; }

        // 使用者Id
        public int UserId { get; set; }


        // 賣家Id
        public int SellerId { get; set; }

        public string? ShopName { get; set; }

        // 產品
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string ProductImg { get; set; }

        public decimal ProductPrice { get; set; }

        // 產品數量
        public int Quantity { get; set; }


    }
}
