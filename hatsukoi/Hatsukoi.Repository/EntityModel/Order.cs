using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Order
{
    /// <summary>
    /// 訂單編號
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 訂單狀態( 1:尚未付款 2:處理中(買家已付款，賣家未出貨) 3:帶收貨 4:已完成 5:已取消(三天內沒付款，或是買家自行取消) 6:退貨申請中 7:已退貨)
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 收件地址
    /// </summary>
    public string RecipientAddress { get; set; } = null!;

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
    /// 優惠券的Id，如果沒使用則為0
    /// </summary>
    public int? CouponId { get; set; }

    /// <summary>
    /// 訂單評價，(評分範圍可為1~5)
    /// </summary>
    public int? Evaluate { get; set; }

    /// <summary>
    /// 付款方式 ( 1:轉帳、2:信用卡 )
    /// </summary>
    public int? Payment { get; set; }

    /// <summary>
    /// 訂單下單時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 訂單付款完成時間
    /// </summary>
    public DateTime? PayTime { get; set; }

    /// <summary>
    /// 綠界Id
    /// </summary>
    public string GreenPayId { get; set; } = null!;

    /// <summary>
    /// 備註欄 當買家取消or退貨時 不為null
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// 訂單評價的留言文字
    /// </summary>
    public string? EvaluateText { get; set; }

    /// <summary>
    /// 收件人的城市
    /// </summary>
    public string RecipientCity { get; set; } = null!;

    /// <summary>
    /// 收件人的郵遞區號
    /// </summary>
    public string RecipientPostCode { get; set; } = null!;

    /// <summary>
    /// 訂單評價的時間
    /// </summary>
    public DateTime? EvaluateDate { get; set; }

    /// <summary>
    /// 訂單進入取消的時間
    /// </summary>
    public DateTime? StatusCancelTime { get; set; }

    /// <summary>
    /// 賣家計出貨物的時間
    /// </summary>
    public DateTime? StatusSendTime { get; set; }

    /// <summary>
    /// 訂單完成時間
    /// </summary>
    public DateTime? StatusFinishTime { get; set; }

    public string OrderNumber { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual Seller Seller { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
