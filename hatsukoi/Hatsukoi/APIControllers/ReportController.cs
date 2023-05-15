using Hatsukoi.Models;
using Hatsukoi.Models.Dtos.Report;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;
        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }


        [HttpPost]
        public IActionResult AAA(ChartDto dto)
        {
            if (dto.First == null || dto.Last == null)
            {
                var a = _reportService.StarSale();
                if (a.Date.Count == 0)
                {
                    return Ok(new APIBaseResponse()
                    {
                        Result = new { result = "success", message = "本帳號尚無成立任何訂單" }
                    });
                }
                return Ok(new APIBaseResponse(a));
            }
            else if (dto != null)
            {
                var b = _reportService.NowMonthSale(dto);
                return Ok(new APIBaseResponse(b));
            }
            return Ok(new APIBaseResponse());

        }

        [HttpGet]
        public IActionResult PayTime()
        {
            var a = _reportService.Order();
            var shipOrder =_reportService.ShipOrder();
            var totalOrder = _reportService.TotalOrder();
            return Ok(new APIBaseResponse()
            {
                Result = new { result = "success", a = a , shipOrder = shipOrder , totalOrder = totalOrder }
            });
        }
        [HttpPost]
        public IActionResult Change(ChartDto dto)
        {

            if (dto.First == null ||dto.Last ==null) 
            {
                var a = _reportService.StarOrderCount();
                return Ok(new APIBaseResponse(a));
            }
            else if(dto != null)
            {
                var b = _reportService.OrderCount(dto);
                return Ok(new APIBaseResponse(b));
            }
            return Ok(new APIBaseResponse());

        }
        [HttpPost]
        public IActionResult Product()
        {
            var a = _reportService.ProductViews();
            return Ok(new APIBaseResponse(a));
        }

        [HttpPost]
        public IActionResult CancelOrder(ChartDto dto)
        {
            

            if (dto.First == null || dto.Last == null)
            {
                var a = _reportService.StarCancelOrderCount();
                return Ok(new APIBaseResponse(a));
            }
            else if (dto != null)
            {
                var b = _reportService.CancelOrderCount(dto);
                return Ok(new APIBaseResponse(b));
            }
            return Ok(new APIBaseResponse());
        }
    }
}
