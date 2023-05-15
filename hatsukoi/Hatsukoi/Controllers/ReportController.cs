using Hatsukoi.Service;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportService _reportService;


        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
           
        }
        public IActionResult Chart()
        {
            //var a = _reportService.NowMouthSale();
            return View();
        }


    }
}
