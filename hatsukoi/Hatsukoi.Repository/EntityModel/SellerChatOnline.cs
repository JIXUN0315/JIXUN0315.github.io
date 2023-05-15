using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class SellerChatOnline
{
    /// <summary>
    /// 賣家線上清單的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 當連線進聊天室時會獲得一組連線ID
    /// </summary>
    public string ConnectionId { get; set; } = null!;

    /// <summary>
    /// 聊天時的Seller身分
    /// </summary>
    public int? SellerId { get; set; }

    public virtual Seller? Seller { get; set; }
}
