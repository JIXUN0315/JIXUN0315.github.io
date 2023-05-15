using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class Admin
{
    /// <summary>
    /// 管理員Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 管理員的使用者ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 管理員名稱
    /// </summary>
    public string Name { get; set; } = null!;

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Reviewer> Reviewers { get; } = new List<Reviewer>();
}
