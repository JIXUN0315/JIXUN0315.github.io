using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Seller
{
    /// <summary>
    /// 商店Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 商店的負責人姓名
    /// </summary>
    public string ApplyName { get; set; } = null!;

    /// <summary>
    /// 商店的負責人電話
    /// </summary>
    public string ApplyPhone { get; set; } = null!;

    /// <summary>
    /// 商店名稱
    /// </summary>
    public string ShopName { get; set; } = null!;

    /// <summary>
    /// 商店負責人的Email
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 商店實際所在位置(縣市)
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// 商品產地
    /// </summary>
    public string ProductOrigin { get; set; } = null!;

    /// <summary>
    /// 商店的社群媒體網址
    /// </summary>
    public string SocialMedia { get; set; } = null!;

    /// <summary>
    /// 品牌介紹文字
    /// </summary>
    public string Introduction { get; set; } = null!;

    /// <summary>
    /// 商店創建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 商店照片logo網址
    /// </summary>
    public string Logo { get; set; } = null!;

    /// <summary>
    /// 修改商店資料時間
    /// </summary>
    public DateTime ModifiDate { get; set; }

    /// <summary>
    /// 他的買家Id
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 使用的幣別(1:台幣 2:美金)
    /// </summary>
    public int? Currency { get; set; }

    /// <summary>
    /// 商店的負責人的身分證字號
    /// </summary>
    public string IdNumber { get; set; } = null!;

    /// <summary>
    /// 商店登記地址
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// 商店的負責人英文姓名
    /// </summary>
    public string? ApplicantEnglishName { get; set; }

    /// <summary>
    /// 商店負責人的第二Email
    /// </summary>
    public string? SecondEmail { get; set; }

    /// <summary>
    /// 商店登記地址的郵遞區號
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// 在休假模式時，休假的第一天
    /// </summary>
    public DateTime? VacationFirstDay { get; set; }

    /// <summary>
    /// 在休假模式時，休假的最後一天
    /// </summary>
    public DateTime? VacationLastDay { get; set; }

    /// <summary>
    /// 店家的設計館招牌(正方形)
    /// </summary>
    public string ShopBannerSquare { get; set; } = null!;

    /// <summary>
    /// 商店的故事(介紹)
    /// </summary>
    public string? Story { get; set; }

    /// <summary>
    /// 店家收款帳號的銀行代碼
    /// </summary>
    public string? BankCode { get; set; }

    /// <summary>
    /// 店家收款帳號的分行代碼
    /// </summary>
    public string? BranchCode { get; set; }

    /// <summary>
    /// 店家收款帳號的銀行帳號
    /// </summary>
    public string? BankAccount { get; set; }

    /// <summary>
    /// 店家收款帳號的戶名
    /// </summary>
    public string? BankAccountName { get; set; }

    /// <summary>
    /// 修改財務信箱與收款帳戶時，需輸入驗證碼證明身分。(在成為賣家時會給)
    /// </summary>
    public string VerificationCode { get; set; } = null!;

    /// <summary>
    /// 授權經銷品牌: hatsukoi
    /// </summary>
    public string BrandName { get; set; } = null!;

    /// <summary>
    /// 統一編號(8碼數字)
    /// </summary>
    public string? TaxIdNumber { get; set; }

    /// <summary>
    /// 商店照片icon網址
    /// </summary>
    public string Icon { get; set; } = null!;

    /// <summary>
    /// 店家的設計館招牌(長方形)
    /// </summary>
    public string ShopBannerRect { get; set; } = null!;

    /// <summary>
    /// 第二組email修改時的暫存
    /// </summary>
    public string? StateSecondEmail { get; set; }

    /// <summary>
    /// 第二組email修改時的時間
    /// </summary>
    public DateTime? StateSecondTime { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual ICollection<Chatroom> Chatrooms { get; } = new List<Chatroom>();

    public virtual ICollection<Coupon> Coupons { get; } = new List<Coupon>();

    public virtual ICollection<FavShop> FavShops { get; } = new List<FavShop>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual ICollection<SellerChatOnline> SellerChatOnlines { get; } = new List<SellerChatOnline>();
}
