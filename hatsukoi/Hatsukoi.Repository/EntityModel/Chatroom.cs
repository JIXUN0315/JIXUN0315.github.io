using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Chatroom
{
    /// <summary>
    /// 聊天訊息ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 誰傳送訊息
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 誰收到訊息
    /// </summary>
    public int SellerId { get; set; }

    /// <summary>
    /// 訊息內容
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// 新增訊息時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 1為User發送的，2為Seller發送的
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// 訊息為 1.已讀 或 2.未讀
    /// </summary>
    public int? IsRead { get; set; }

    public virtual Seller Seller { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
