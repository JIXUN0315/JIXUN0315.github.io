using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Models.ViewModels.Cart
{
    public class CreateOrderViewModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNumber { get; set; } 

        /// <summary>
        /// 訂單狀態( 1:尚未付款 2:處理中(買家已付款，賣家未出貨) 3:帶收貨 4:已完成 5:已取消(三天內沒付款，或是買家自行取消) 6:退貨申請中 7:已退貨)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        public string RecipientAddress { get; set; } 

        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string RecipientName { get; set; } = null!;

        /// <summary>
        /// 收件人電話
        /// </summary>
        public string RecipientPhone { get; set; } = null!;

        /// <summary>
        /// 訂單最終結帳金額
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 賣家Id ( 因為不能合併結帳，所以一張單只會有一個賣家 )
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 哪個帳號下訂單的
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///// 優惠券的Id，如果沒使用則為0
        /// </summary>
        public int CouponId { get; set; }

        public decimal DiscountAmount { get; set; }

        
        /// <summary>
        /// 付款方式 ( 1:轉帳、2:信用卡 )
        /// </summary>
        public int? Payment { get; set; }

        /// <summary>
        /// 訂單下單時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        

        /// <summary>
        /// 綠界Id
        /// </summary>
        public string GreenPayId { get; set; } 

        
        /// <summary>
        /// 收件人的城市
        /// </summary>
        public string RecipientCity { get; set; } 

        /// <summary>
        /// 收件人的郵遞區號
        /// </summary>
        public string RecipientPostCode { get; set; }





    }
}
