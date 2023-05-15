namespace Hatsukoi.Models.ViewModels.ChatRoom
{
    public class Receivers
    {
        public string Name { get; set; }
        public string Img { get; set; }

    }
    public class ChatViewModel
    {
        public string ReceiverImg { get; set; }
        public string ReceiverName { get; set; }
        public int ReceiverId { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageTime { get; set; }
        public int UnRead { get; set; }
    }

}
