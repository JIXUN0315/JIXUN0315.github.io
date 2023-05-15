using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Notification
{
    /// <summary>
    /// 通知訊息的編號
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 誰接收訊息(來自UserId)
    /// </summary>
    public int SendTo { get; set; }

    /// <summary>
    /// 通知發送時間
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 訊息內文(手風琴拉開後的顯示)
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// 訊息標題(手風琴未拉開的顯示)
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 連結的網址(訊息內的button按鈕)
    /// </summary>
    public string LinkTo { get; set; } = null!;

    /// <summary>
    /// 訊息的種類是哪一種(1:帳號通知)(2:訂單通知)
    /// </summary>
    public int KindOfNotifi { get; set; }

    public string? SendImg { get; set; }

    /// <summary>
    /// 訊息為 1.已讀 或 2.未讀
    /// </summary>
    public int? IsRead { get; set; }

    public virtual User SendToNavigation { get; set; } = null!;
}
