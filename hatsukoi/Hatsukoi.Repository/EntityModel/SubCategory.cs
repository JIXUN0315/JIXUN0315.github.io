using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class SubCategory
{
    /// <summary>
    /// 商品小分類Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 商品大分類的Id
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 產品小分類的名稱
    /// </summary>
    public string SubCategoryName { get; set; } = null!;

    /// <summary>
    /// 資料最後更改時間
    /// </summary>
    public DateTime EditLastTime { get; set; }

    /// <summary>
    /// 最後更改資料的
    /// </summary>
    public int UserId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
