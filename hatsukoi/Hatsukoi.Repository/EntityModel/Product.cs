using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Product
{
    /// <summary>
    /// 產品編號
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 產品名稱
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary>
    /// 產品小分類Id(產品只會連到小分類，小分類才會連到大分類)
    /// </summary>
    public int SubCategory { get; set; }

    /// <summary>
    /// 產品價格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 1:未上架 2.已上架
    /// </summary>
    public int ProductStatus { get; set; }

    /// <summary>
    /// 誰賣這個商品
    /// </summary>
    public int SellerId { get; set; }

    /// <summary>
    /// 總瀏覽數
    /// </summary>
    public int? ViewTimes { get; set; }

    /// <summary>
    /// 商品產地
    /// </summary>
    public string? MadeCountry { get; set; }

    /// <summary>
    /// 商品的描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 商品介紹，用富文本編輯器轉HTML來儲存
    /// </summary>
    public string Editor { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual ICollection<FavProduct> FavProducts { get; } = new List<FavProduct>();

    public virtual ICollection<ProductImg> ProductImgs { get; } = new List<ProductImg>();

    public virtual ICollection<ProductSpecification> ProductSpecifications { get; } = new List<ProductSpecification>();

    public virtual ICollection<ProductTagList> ProductTagLists { get; } = new List<ProductTagList>();

    public virtual Seller Seller { get; set; } = null!;

    public virtual SubCategory SubCategoryNavigation { get; set; } = null!;

    public virtual ICollection<ViewTime> ViewTimesNavigation { get; } = new List<ViewTime>();
}
