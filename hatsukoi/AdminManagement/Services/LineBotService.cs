using AdminManagement.Models.ViewModels;
using isRock.LineBot.Extensions;
using Line.Messaging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static AdminManagement.Models.ViewModels.Message;

namespace AdminManagement.Services
{
    public class LineBotService
    {
        private readonly string broadcastMessageUri = "https://api.line.me/v2/bot/message/broadcast";
        private static HttpClient client = new HttpClient(); // 負責處理HttpRequest

        /// <summary>
        /// 將廣播訊息請求送到 Line
        /// </summary>        
        public async void BoardcastMessage(LineCreateViewModel request)
        {

            var channelAccessToken = "yM/Ch11+VKOcbBpCRLxwvbHXqxNhq0qs4IjMBnKd/fEtPwE0sFwAbR2YS8WfdU0ZO08LWmAOnQNs4xBKh0hXTKRtSECXCl9+UUXGivCnLZBTUQseKjLmkLVCXJzyXj0kVxwawBf9/y7V2PbgZ2nXCAdB04t89/1O/w1cDnyilFU=";

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
            //var message = new Message
            //{
            //    Messages = new List<Message.MessageObj>
            //    {
            //        new Message.MessageObj
            //        {
            //            Type = "text",
            //            Text = request.Text
            //        }
            //    }
            //};

            var templateMessage = new Message
            {
                Messages = new List<Message.MessageObj>
                {
                    new Message.MessageObj
                    {
                        Type = "template",
                        AltText = request.Text,
                        Template = new Template
                        {
                            Type = "buttons",
                            ThumbnailImageUrl = "https://hatsukoifront.azurewebsites.net/hatsukoiPics/hatsukoi_logo.png",
                            ImageAspectRatio = "rectangle",
                            ImageSize = "cover",
                            ImageBackgroundColor = "#FFFFFF",
                            Text = request.Text,
                            Actions = new List<Template.ActionObj>
                            {
                                new Template.ActionObj
                                {
                                    Type = "uri",
                                    Label = "立即前往",
                                    Uri = "https://hatsukoifront.azurewebsites.net/"
                                }

                            }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(templateMessage, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(broadcastMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
