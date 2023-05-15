using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Reviewer
{
    /// <summary>
    /// user成為賣家的審核表 ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 誰申請的
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 何時審核的
    /// </summary>
    public DateTime? ReviewTime { get; set; }

    /// <summary>
    /// 審核狀態 0:未審核 1:審核失敗 2:審核成功
    /// </summary>
    public int Reviewstatus { get; set; }

    /// <summary>
    /// 審核失敗時，失敗的原因
    /// </summary>
    public string? FailReason { get; set; }

    /// <summary>
    /// 申請人姓名
    /// </summary>
    public string ApplyName { get; set; } = null!;

    /// <summary>
    /// 申請人手機號碼
    /// </summary>
    public string ApplyPhone { get; set; } = null!;

    /// <summary>
    /// 品牌名稱 (設計館名稱)
    /// </summary>
    public string ShopName { get; set; } = null!;

    /// <summary>
    /// 商店負責人的Email
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 商品產地
    /// </summary>
    public string ProductOrigin { get; set; } = null!;

    /// <summary>
    /// 商店實際所在位置(縣市)
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// 商店的社群媒體網址
    /// </summary>
    public string SocialMedia { get; set; } = null!;

    /// <summary>
    /// 品牌介紹文字
    /// </summary>
    public string Introduction { get; set; } = null!;

    /// <summary>
    /// 審核者Id
    /// </summary>
    public int? AdministratorId { get; set; }

    /// <summary>
    /// 申請表提交時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 申請人Id
    /// </summary>
    public string IdNumber { get; set; } = null!;

    public string? BrandName { get; set; }

    public DateTime? LastEditTime { get; set; }

    public virtual Admin? Administrator { get; set; }

    public virtual ICollection<ApplyImg> ApplyImgs { get; } = new List<ApplyImg>();

    public virtual User User { get; set; } = null!;
}
