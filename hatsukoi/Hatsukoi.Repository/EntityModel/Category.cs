using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Category
{
    /// <summary>
    /// 產品大分類的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 產品大分類的名稱
    /// </summary>
    public string CategoryName { get; set; } = null!;

    /// <summary>
    /// 資料最後更改時間
    /// </summary>
    public DateTime EditLastTime { get; set; }

    /// <summary>
    /// 最後更改資料的
    /// </summary>
    public int UserId { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; } = new List<SubCategory>();
}
