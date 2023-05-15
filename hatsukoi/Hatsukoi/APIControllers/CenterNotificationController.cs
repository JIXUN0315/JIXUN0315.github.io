using Hatsukoi.Hubs;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Hatsukoi.APIControllers
{
    public class CenterNotificationController : BaseApiController
    {
        //private readonly IHubContext<ChatHub> _hubContext;
        private readonly CenterNotificationService _service;

        public CenterNotificationController(CenterNotificationService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> SendNotice()
        {
            await _service.SendMsg();
            return Ok();
        }
    }
}
