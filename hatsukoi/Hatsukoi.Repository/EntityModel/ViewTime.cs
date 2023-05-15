using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ViewTime
{
    /// <summary>
    /// 觀看次數記錄的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 誰觀看這個商品
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 被觀看的商品Id
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// 瀏覽的時間
    /// </summary>
    public DateTime Time { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
