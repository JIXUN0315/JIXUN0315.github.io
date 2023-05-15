namespace Hatsukoi.Models.ViewModels.User
{
    public class UserOrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string ShopName { get; set; }
        public int OrderStatus { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal CouponCount { get; set; }
        public string OrderNumber { get; set; }
        public string FirstImg { get; set; }
        public string RecipientCity { get; set; }
        public int SellerId { get; set; }
        public bool ExistEvaluate { get; set; }
        public List<OrderDetailListViewModel> OrderDetailItems { get; set; }
    }
    public class OrderDetailListViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string FirstSepcItem { get; set; }
        public string SecondSepcItem { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductImg { get; set; }
    }
}
