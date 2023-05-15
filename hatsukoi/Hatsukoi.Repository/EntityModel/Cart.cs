using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Cart
{
    /// <summary>
    /// 購物車Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 使用者Id
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 賣家Id
    /// </summary>
    public int SellerId { get; set; }

    /// <summary>
    /// 產品編號
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// 產品數量
    /// </summary>
    public int Quantity { get; set; }

    public int ProductSpecificationId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Seller Seller { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
