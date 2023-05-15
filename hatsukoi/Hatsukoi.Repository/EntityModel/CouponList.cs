using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class CouponList
{
    /// <summary>
    /// 票券的Id
    /// </summary>
    public int CouponId { get; set; }

    /// <summary>
    /// 票券的擁有者
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 票券狀態 ( 0:還不能使用 1:超過使用期限 2:可使用 3:已被使用 )
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 票券得到日期
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 誰擁有優惠券的清單的Id
    /// </summary>
    public int Id { get; set; }

    public virtual Coupon Coupon { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
