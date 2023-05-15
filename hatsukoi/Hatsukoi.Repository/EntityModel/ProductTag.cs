using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ProductTag
{
    /// <summary>
    /// 商品標籤Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 標籤名稱
    /// </summary>
    public string TagName { get; set; } = null!;

    /// <summary>
    /// 最後更改資料的
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 資料最後更改時間
    /// </summary>
    public DateTime EditLastTime { get; set; }

    public virtual ICollection<ProductTagList> ProductTagLists { get; } = new List<ProductTagList>();
}
