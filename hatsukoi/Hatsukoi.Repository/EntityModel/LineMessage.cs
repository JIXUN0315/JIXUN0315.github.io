using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class LineMessage
{
    /// <summary>
    /// 管理員後台發出Line給所有買家的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 哪個管理員發出的
    /// </summary>
    public int? AdminId { get; set; }

    /// <summary>
    /// 通知發送時間
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 訊息內文(手風琴拉開後的顯示)
    /// </summary>
    public string? Text { get; set; }

    public bool? IsSend { get; set; }
}
