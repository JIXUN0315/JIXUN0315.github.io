using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ProductImg
{
    /// <summary>
    /// 商品圖片Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 圖片本身(網址)
    /// </summary>
    public string Img { get; set; } = null!;

    /// <summary>
    /// 這個圖片屬於哪個商品
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// 商品排序順位(同一個商品的照片排序，必須四張)
    /// </summary>
    public int Sort { get; set; }

    public virtual Product Product { get; set; } = null!;
}
