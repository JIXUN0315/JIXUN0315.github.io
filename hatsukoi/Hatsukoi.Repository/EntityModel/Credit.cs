using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Credit
{
    /// <summary>
    /// 信用卡清單的Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 信用卡卡號
    /// </summary>
    public string CreditNumber { get; set; } = null!;

    /// <summary>
    /// 信用卡持有者的使用者Id
    /// </summary>
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
