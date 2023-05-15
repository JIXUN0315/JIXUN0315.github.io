using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class FavProduct
{
    /// <summary>
    /// 喜好商品的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// user的ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// product的id
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// 加入清單時間
    /// </summary>
    public DateTime Time { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
