using System;
using System.Collections.Generic;

namespace Hatsukoi.Repository.EntityModel;

public partial class ApplyImg
{
    /// <summary>
    /// 申請賣家要附五張照片，存這些照片的Id的表
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 哪一次申請的Id
    /// </summary>
    public int ReviewerId { get; set; }

    /// <summary>
    /// 照片的網址
    /// </summary>
    public string ImgUrl { get; set; } = null!;

    public virtual Reviewer Reviewer { get; set; } = null!;
}
