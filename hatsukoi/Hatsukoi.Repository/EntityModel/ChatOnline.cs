using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ChatOnline
{
    /// <summary>
    /// 當連線進聊天室時會獲得一組連線ID
    /// </summary>
    public string ConnectionId { get; set; } = null!;

    /// <summary>
    /// 聊天時的User身分
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 聊天室線上人員資料表的Id
    /// </summary>
    public int Id { get; set; }

    public virtual User User { get; set; } = null!;
}
