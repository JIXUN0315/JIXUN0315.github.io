using Hatsukoi.Repository.EntityModel;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Models.ViewModels.OrderVM
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusText { get; set; }
        public string RecipientAddress { get; set; } = null!;
        public string RecipientName { get; set; } = null!;
        public string RecipientPhone { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public int SellerId { get; set; }
        public int UserId { get; set; }
        public int? CouponId { get; set; }
       
        public int? Evaluate { get; set; }
        public int? Payment { get; set; }
        public string CreateTime { get; set; }
        public string? PayTime { get; set; }
        public string? Memo { get; set; }

        public string? EvaluateText { get; set; }
        public string? EvaluateDate { get; set; }
        public string? StatusCancelTime { get; set; }
        public string? StatusSendTime { get; set; }
        public string? StatusFinishTime { get; set; }
        public List<OrderDetailsViewModel> OrderDetails { get; internal set; }
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public string? OrderNumber { get; set; }
        public int TotalQuantity { get; set; }
        public bool Selected { get; set; }
        public string MemberLevel { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountNum { get; set; }
        public decimal OriginalPrice { get;set; }
    }

}
