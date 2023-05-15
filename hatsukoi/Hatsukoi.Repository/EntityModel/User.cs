using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class User
{
    /// <summary>
    /// 使用者ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User姓名
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 使用者的性別，0:男,1:女
    /// </summary>
    public bool? Gender { get; set; }

    /// <summary>
    /// 使用者加入Hatsukoi日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 使用者的頭貼
    /// </summary>
    public string? Photo { get; set; }

    /// <summary>
    /// 使用者的信箱(第三方登入的話也會有信箱)
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 信箱是否認證通過(第三方登入也會有信箱，會判定為已驗證)
    /// </summary>
    public bool IsEmailCertified { get; set; }

    /// <summary>
    /// 帳號，(因為會有第三方登入，所以設為可為NULL)
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 密碼，(因為會有第三方登入，所以設為可為NULL)
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 第三方登入會有一組識別碼，有這個就不會有帳號密碼
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// 使用者電話
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 使用者的生日
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 會員等級，0:品藍,1:白銀,2:黃金,3:鑽石,4:尊爵
    /// </summary>
    public int MemberLevel { get; set; }

    /// <summary>
    /// 店家狀態,0:沒申請,1:審核中,2:駁回,3:通過,4:停權
    /// </summary>
    public int StoreStatus { get; set; }

    /// <summary>
    /// 修改使用者資料時間
    /// </summary>
    public DateTime ModifiDate { get; set; }

    /// <summary>
    /// 最後上線時間
    /// </summary>
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 電子郵件 訂單狀態 通知設定
    /// </summary>
    public bool IsEmailOrder { get; set; }

    /// <summary>
    /// 電子郵件 站內訊息 通知設定
    /// </summary>
    public bool IsEmailHatsukoi { get; set; }

    /// <summary>
    /// 電子郵件 活動優惠 通知設定
    /// </summary>
    public bool IsEmailActivity { get; set; }

    /// <summary>
    /// 電子郵件 個人化推薦 通知設定
    /// </summary>
    public bool IsEmailPersonal { get; set; }

    /// <summary>
    /// 電子郵件 會員雙周報 通知設定
    /// </summary>
    public bool IsEmailWeek { get; set; }

    /// <summary>
    /// 電子郵件 會員權益 通知設定
    /// </summary>
    public bool IsEmailMember { get; set; }

    /// <summary>
    /// 電子郵件 追蹤的商品和設計館 通知設定
    /// </summary>
    public bool IsEmailFollow { get; set; }

    /// <summary>
    /// 電子郵件關注評價優惠券 通知設定
    /// </summary>
    public bool IsEmailFocus { get; set; }

    /// <summary>
    /// 寄驗證信時間
    /// </summary>
    public DateTime? CertifiedTime { get; set; }

    /// <summary>
    /// 寄重設密碼信時間
    /// </summary>
    public DateTime? ResetPasswordTime { get; set; }

    /// <summary>
    /// FB第三方登入的id
    /// </summary>
    public string? IdentifierFb { get; set; }

    /// <summary>
    /// Line第三方登入的id
    /// </summary>
    public string? IdentifierLine { get; set; }

    public bool? IsEmailRigister { get; set; }

    public DateTime? EmailRigisterTime { get; set; }

    /// <summary>
    /// 修改Email時暫存新的email
    /// </summary>
    public string? NewEmail { get; set; }

    /// <summary>
    /// 寄出修改email信時的時間記錄
    /// </summary>
    public DateTime? ChangeEmailTime { get; set; }

    /// <summary>
    /// 使用者進入通知中心的時間
    /// </summary>
    public DateTime? LookNotifyTime { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual ICollection<ChatOnline> ChatOnlines { get; } = new List<ChatOnline>();

    public virtual ICollection<Chatroom> Chatrooms { get; } = new List<Chatroom>();

    public virtual ICollection<CouponList> CouponLists { get; } = new List<CouponList>();

    public virtual ICollection<Credit> Credits { get; } = new List<Credit>();

    public virtual ICollection<FavProduct> FavProducts { get; } = new List<FavProduct>();

    public virtual ICollection<FavShop> FavShops { get; } = new List<FavShop>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Reviewer> Reviewers { get; } = new List<Reviewer>();

    public virtual ICollection<ViewTime> ViewTimes { get; } = new List<ViewTime>();
}
