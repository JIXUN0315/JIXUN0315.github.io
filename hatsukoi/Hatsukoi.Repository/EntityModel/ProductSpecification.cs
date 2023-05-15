using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ProductSpecification
{
    /// <summary>
    /// 商品規格的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 商品第一種規格名稱(標題)
    /// </summary>
    public string FirstSpecification { get; set; } = null!;

    /// <summary>
    /// 商品第一種規格的選項
    /// </summary>
    public string FirstSepcItem { get; set; } = null!;

    /// <summary>
    /// 商品第二種規格名稱(標題)
    /// </summary>
    public string? SecondSpecification { get; set; }

    /// <summary>
    /// 商品第二種規格的選項
    /// </summary>
    public string? SecondSepcItem { get; set; }

    /// <summary>
    /// 商品的Id
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// 這個規格如果有與Product價格不同時將會存在這個欄位
    /// </summary>
    public decimal? UnitPrice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual Product Product { get; set; } = null!;
}
