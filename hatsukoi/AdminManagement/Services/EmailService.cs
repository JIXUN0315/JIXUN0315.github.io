using AdminManagement.Models.Dtos;
using MimeKit;
using MailKit.Net.Smtp;

namespace AdminManagement.Services
{
    public class EmailService
    {
        public void Send(EmailDto emailDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hatsukoi", "hatsukoi168@gmail.com"));
            message.To.Add(new MailboxAddress(emailDto.Name, emailDto.Email));
            message.Subject = GetEmailText(emailDto).Item2;

            message.Body = new TextPart("html")
            {
                Text = GetEmailText(emailDto).Item1
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("hatsukoi168@gmail.com", "hagujesdogsflrop");

                client.Send(message);
                client.Disconnect(true);
            }
        }
        //emailType
        //0:傳送賣家申請通過通知
        //1:傳送賣家申請駁回通知
        private (string, string) GetEmailText(EmailDto emailDto)
        {
            string url;
            string text = "";
            string title = "";
            switch (emailDto.EmailType)
            {
                case 0:
                    url = $"https://hatsukoifront.azurewebsites.net/";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好， 謝謝你申請在 Hatsukoi 開立專屬設計館 </p>
                    <p>您的 專屬驗證碼 為 : {emailDto.Account}</p>
                    <p>專屬驗證碼 為之後修改賣家資料時必要的資訊。</p>
                    <p>再次謝謝你的申請和耐心等待，期待迎接你加入 Hatsukoi！";
                    title = "成功成為賣家";
                    return (text, title);
                case 1:
                    url = $"https://hatsukoifront.azurewebsites.net/";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>謝謝你申請在 Hatsukoi 平台上開立專屬設計館，我們已完成審核。 </p>
                    <p>由於以下因素，Hatsukoi 暫時不會發出開館授權，敬請見諒。</p>
                    <p>{emailDto.Reason}</p>
                    <p>歡迎你再次遞交申請表，謝謝。</p>";
                    title = "賣家申請駁回";
                    return (text, title);
                case 2:
                    url = $"https://hatsukoifront.azurewebsites.net/";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>您的賣家身分已被停權，因為違反我們的規定。 </p>
                    <p>由於以下因素，Hatsukoi 將停權您的賣家身分，敬請見諒。</p>
                    <p>{emailDto.Reason}</p>
                    <p>如有疑慮可以來信 hatsukoi168@gmail.com ，謝謝。</p>";
                    title = "賣家帳號停權";
                    return (text, title);
                case 3:
                    url = $"https://hatsukoifront.azurewebsites.net/";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>您的賣家身分停權已經復原囉! </p>
                    <p>再次謝謝你的耐心等待，期待你加入 Hatsukoi！</p>
                    <p>如有疑慮可以來信 hatsukoi168@gmail.com ，謝謝。</p>";
                    title = "賣家身分復原成功";
                    return (text, title);
                default:
                    return (text, title);
            }
        }
    }
}
