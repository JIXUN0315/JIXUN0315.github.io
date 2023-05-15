using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Coupon
{
    /// <summary>
    /// 優惠券的Id ( 由賣家發行的優惠券種類表 )
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 優惠券由哪個賣家發行
    /// </summary>
    public int SellerId { get; set; }

    /// <summary>
    /// 折扣數
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// 使用說明 ( 使用條件 )
    /// </summary>
    public decimal Condition { get; set; }

    /// <summary>
    /// 優惠券建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 優惠券開始使用時間
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 優惠券最後使用時間
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 自訂的優惠碼
    /// </summary>
    public string PromoCode { get; set; } = null!;

    /// <summary>
    /// 優惠券是否被刪除了(0:未刪除 1:已刪除)
    /// </summary>
    public int? DeleteStatus { get; set; }

    public virtual ICollection<CouponList> CouponLists { get; } = new List<CouponList>();

    public virtual Seller Seller { get; set; } = null!;
}
