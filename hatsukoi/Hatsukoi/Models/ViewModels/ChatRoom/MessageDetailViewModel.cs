namespace Hatsukoi.Models.ViewModels.ChatRoom
{
    public class MessageDetailViewModel
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public List<MessageDetail> Messages { get; set; }
    }
    public class MessageDetail
    {
        public int MsgType { get; set; }
        public string Message { get; set; }
        public string SendTime { get; set; }
        public int MsgId { get; set; }
        public string ChangeTime { get; set; }
    }
    public class StarMessage
    {
        public int ReciverId { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Messages { get; set; }
    }
}
