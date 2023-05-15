using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Banner
{
    /// <summary>
    /// banner圖片Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 圖片網址
    /// </summary>
    public string ImgUrl { get; set; } = null!;

    /// <summary>
    /// 圖片順序
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 圖片狀態(1:顯示,0:不顯示)
    /// </summary>
    public int Status { get; set; }
}
