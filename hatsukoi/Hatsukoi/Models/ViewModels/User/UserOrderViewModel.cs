using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Models.ViewModels.User
{
    public class UserOrderViewModel
    {
        public int OrderId { get; set; }
        public string ShopName { get; set; }
        public int OrderStatus { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstImg { get; set; }
        public decimal CouponCount { get; set; }
        public string OrderNumber { get; set; }
        public string RecipientCity { get; set; }
        public int SellerId { get; set; }
    }
    public class UserAllOrder
    {
        public List<UserOrderViewModel> NotPayList { get; set; }
        public List<UserOrderViewModel> NotShippedList { get; set; }
        public List<UserOrderViewModel> ToBeReceivedList { get; set; }
        public List<UserOrderViewModel> CompletedList { get; set; }
        public List<UserOrderViewModel> CancelledList { get; set; }
    }
}
