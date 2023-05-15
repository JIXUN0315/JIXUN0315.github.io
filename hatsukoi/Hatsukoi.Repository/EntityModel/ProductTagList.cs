using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ProductTagList
{
    /// <summary>
    /// 商品標籤的清單
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 商品標籤Id
    /// </summary>
    public int ProductTagId { get; set; }

    /// <summary>
    /// 商品Id
    /// </summary>
    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ProductTag ProductTag { get; set; } = null!;
}
