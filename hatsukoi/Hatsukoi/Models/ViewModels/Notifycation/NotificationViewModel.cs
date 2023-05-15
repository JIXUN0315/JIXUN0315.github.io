namespace Hatsukoi.Models.ViewModels.Notifycation
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string SendTime { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string LinkTo { get; set; }
        public string SendImg { get; set; }

    }
    public class NotificationOrderDto
    {
        public int Id { get; set; }
        public DateTime SendTime { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string LinkTo { get; set; }
        public string SendImg { get; set; }

    }
    public class NotificationListVM
    {
        public List<NotificationViewModel> Items { get; set;}
        public int TotalPage { get; set; }
        public int ThisPage { get; set; }
        public int notifyType { get; set; }
    }
}
