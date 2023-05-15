namespace Hatsukoi.Models.Dtos.ChatDto
{
    public class SendMessageDto
    {
        public string Message { get; set; }
        public int SendFromUserId { get; set; }
        public string Time { get; set; }
    }
}
