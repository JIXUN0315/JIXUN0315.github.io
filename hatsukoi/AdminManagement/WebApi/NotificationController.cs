using AdminManagement.BaseModels;
using AdminManagement.Models.Dtos;
using AdminManagement.Models.ViewModels;
using AdminManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminManagement.WebApi
{
    public class NotificationController : BaseApiController
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _notificationService.GetAll();
            return Ok(new BaseApiResponse(data));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CenterNotifyCreateViewModel data)
        {
            var result = false;
            if(data.SendTime == "")
            {
                using var client = new HttpClient();
                var referer = Request.Headers.Referer.ToString();
                var url = "https://hatsukoifront.azurewebsites.net/api/CenterNotification/SendNotice";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = _notificationService.Create(data);
                }
            }
            else
            {
                result = _notificationService.Create(data);
            }
            return Ok(new BaseApiResponse() { IsSuccess = result });
        }

        [HttpPost]
        public IActionResult Edit(CenterNotifyEditViewModel data)
        {
            var result = _notificationService.Update(data);
            return Ok(new BaseApiResponse() { IsSuccess = result });
        }

        [HttpPost]
        public IActionResult Delete(NotificationDto data)
        {
            var result = _notificationService.Delete(data.Id);
            return Ok(new BaseApiResponse() { IsSuccess= result });
        }
    }
}
