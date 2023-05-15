using Hatsukoi.Models;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotifyService _notifyService;

        public NotificationController(NotifyService notifyService)
        {
            _notifyService = notifyService;
        }
        [HttpGet]
        public APIBaseResponse ReadNotify()
        {
            try
            {
                _notifyService.ReadNotify();
                var response = new APIBaseResponse();
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }
    }
}
