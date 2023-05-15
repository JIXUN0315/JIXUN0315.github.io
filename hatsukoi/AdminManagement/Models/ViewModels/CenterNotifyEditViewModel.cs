namespace AdminManagement.Models.ViewModels
{
    public class CenterNotifyEditViewModel
    {
        /// <summary>
        /// 管理員後台發出通知給所有買家的Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 通知發送時間
        /// </summary>
        public string SendTime { get; set; }

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

    }
}
