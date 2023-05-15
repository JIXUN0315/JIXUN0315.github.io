using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class OrderDetail
{
    /// <summary>
    /// 訂單明細編號
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 訂單標號
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// 商品規格Id ( 一個明細只能記錄一種商品，規格不同也算不同商品 )
    /// </summary>
    public int ProductSpecificationId { get; set; }

    /// <summary>
    /// 商品個數數量
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 商品單數原價(紀錄)
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 店家名稱(紀錄)
    /// </summary>
    public string SellerName { get; set; } = null!;

    /// <summary>
    /// 商品名稱(紀錄)
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary>
    /// 第一規格標題名稱(紀錄)
    /// </summary>
    public string FirstSpecification { get; set; } = null!;

    /// <summary>
    /// 第一規格選項名稱(紀錄)
    /// </summary>
    public string FirstSepcItem { get; set; } = null!;

    /// <summary>
    /// 第二規格標題名稱(紀錄)
    /// </summary>
    public string? SecondSpecification { get; set; }

    /// <summary>
    /// 第二規格選項名稱(紀錄)
    /// </summary>
    public string? SecondSepcItem { get; set; }

    /// <summary>
    /// 商品圖片
    /// </summary>
    public string ProductImg { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ProductSpecification ProductSpecification { get; set; } = null!;
}
