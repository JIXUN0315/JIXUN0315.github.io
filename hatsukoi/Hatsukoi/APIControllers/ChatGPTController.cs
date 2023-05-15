using Hatsukoi.Models;
using Hatsukoi.Models.Dtos.ChatDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using OpenAI_API;
using OpenAI_API.Completions;

namespace Hatsukoi.APIControllers
{
    public class ChatGPTController : BaseApiController
    {
        [HttpPost]
        public APIBaseResponse AskGPT(GptDto dto)
        {
            string apiKey = "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";
            string answer = string.Empty;
            var openai = new OpenAIAPI(apiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = dto.Message;
            completion.MaxTokens = 4000;
            var result = openai.Completions.CreateCompletionAsync(completion);
            if (result != null)
            {
                foreach (var item in result.Result.Completions)
                {
                    answer = item.Text;
                }
                return new APIBaseResponse(answer);
            }
            else
            {
                return new APIBaseResponse("Explain more about your Query");
            }
        }
    }
}
