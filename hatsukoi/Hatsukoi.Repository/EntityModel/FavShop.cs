using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class FavShop
{
    /// <summary>
    /// 喜好商家的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// user的ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// seller的Id
    /// </summary>
    public int SellerId { get; set; }

    /// <summary>
    /// 加入清單時間
    /// </summary>
    public DateTime Time { get; set; }

    public virtual Seller Seller { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
