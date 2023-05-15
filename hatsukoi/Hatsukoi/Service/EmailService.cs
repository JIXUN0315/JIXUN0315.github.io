using Hatsukoi.Common;
using Hatsukoi.Models.Dtos.Email;
using Hatsukoi.Service.Interface;
using MailKit.Net.Smtp;
using MimeKit;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Service
{
    public class EmailService : IEmailService
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
        private (string,string) GetEmailText(EmailDto emailDto )
        {
            int id = HasherExtension.SetRandomNum(emailDto.Id);
            string url;
            string text = "";
            string title = "";
            switch (emailDto.EmailType)
            {
                case EmailType.CheckEmail:
                    url = $"https://hatsukoifront.azurewebsites.net/Login/Certified/{id}";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好！我們收到你在 Hatsukoi 註冊的要求，為了確認這真的是你，請點擊下方連結以完成註冊：</p>
                    <div class=""btn""><a href=""{url}"">完成註冊</a></div>
                    <p>點擊上方連結即可完成註冊</p>";
                    title = "歡迎加入 Hatsukoi !!!";
                    return (text,title);
                case EmailType.ResetPassword:
                    url = $"https://hatsukoifront.azurewebsites.net/Login/ResetPassword/{id}";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好！你的 Hatsukoi 帳號是 : </p>
                    <p>{emailDto.Account}</p>
                    <p>我們收到你在 Hatsukoi 密碼重設的要求，為了確認這真的是你，請點擊下方連結以進行密碼重設：</p>
                    <div class=""btn""><a href=""{url}"">點擊前往重設</a></div>
                    <p>點擊上方連結即可重設密碼</p>";
                    title = "重設密碼";
                    return (text, title);
                case EmailType.ApplySeller:
                    
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好， 謝謝你申請在 Hatsukoi 開立專屬設計館 </p>
                    <p>提醒你，申請一旦提交即無法變更。如需變更提交的申請資訊，請重新填寫申請表。</p>
                    <p>申請結果會於 10 個工作天內（不含國定節假日）寄送至你填寫的 Email 信箱。</p>
                    <p>再次謝謝你的申請和耐心等待，期待迎接你加入 Hatsukoi！";
                    title = "申請成為賣家";
                    return (text, title);
                case EmailType.BeSellerSuccess:
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好， 謝謝你申請在 Hatsukoi 開立專屬設計館 </p>
                    <p>您的 專屬驗證碼 為 : {emailDto.Account}</p>
                    <p>專屬驗證碼 為之後修改賣家資料時必要的資訊。</p>
                    <p>再次謝謝你的申請和耐心等待，期待迎接你加入 Hatsukoi！";
                    title = "成功成為賣家";
                    return (text, title);
                case EmailType.BindEmail:
                    url = $"https://hatsukoifront.azurewebsites.net/Login/NormalCreateWhenExternal/{id}";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好， 謝謝你申請 Hatsukoi 的帳號 </p>
                    <p>按下以下連結前往註冊。</p>
                    <p>再次謝謝你的申請和耐心等待，期待迎接你加入 Hatsukoi！
                    <div class=""btn""><a href=""{url}"">點擊前往註冊</a></div>";
                    title = "註冊 Hatsukoi 帳號";
                    return (text, title);
                case EmailType.ChangeEmail:
                    url = $"https://hatsukoifront.azurewebsites.net/User/ChangeEmail/{id}";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好，這是您重設Email的信件 </p>
                    <p>按下以下連結前往確認新的Email。</p>
                    <p>再次謝謝你的申請和耐心等待，期待你加入 Hatsukoi！
                    <div class=""btn""><a href=""{url}"">點擊前往確認信箱</a></div>";
                    title = "重設 Hatsukoi 帳號的 Email";
                    return (text, title);
                case EmailType.EditSellerSecondEmail:
                    url = $"https://hatsukoifront.azurewebsites.net/Seller/UpdateSecondEmail/{id}";
                    text = @$"<h1><img src=""https://ppt.cc/fA71Kx@.png""></h1>
                    <p>你好，這是您的設計館通知信箱更新驗證信件 </p>
                    <p>按下以下連結前往確認新的Email。</p>
                    <p>再次謝謝你的申請和耐心等待，期待你加入 Hatsukoi！
                    <div class=""btn""><a href=""{url}"">點擊前往確認信箱</a></div>";
                    title = "重設 Hatsukoi 帳號的 Email";
                    return (text, title);
                default:
                    return (text, title);
            }
        }
    }
}
