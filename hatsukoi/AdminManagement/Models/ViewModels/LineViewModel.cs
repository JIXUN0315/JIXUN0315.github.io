namespace AdminManagement.Models.ViewModels
{
    public class LineViewModel
    {
        /// <summary>
        /// 管理員後台發出通知給所有買家的Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 通知發送時間
        /// </summary>
        public string SendTime { get; set; }

        public bool IsSend { get; set; }

        /// <summary>
        /// 訊息內文(手風琴拉開後的顯示)
        /// </summary>
        public string? Text { get; set; }
        public string? Img { get; set; }


    }
}
