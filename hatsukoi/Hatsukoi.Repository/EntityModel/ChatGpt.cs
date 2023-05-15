using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ChatGpt
{
    public int Id { get; set; }

    public int UserIdOrSellerId { get; set; }

    /// <summary>
    /// 0:GTP對User 1:GTP對Seller
    /// </summary>
    public int MessageType { get; set; }

    public DateTime CreateDate { get; set; }

    public string Message { get; set; } = null!;
}
