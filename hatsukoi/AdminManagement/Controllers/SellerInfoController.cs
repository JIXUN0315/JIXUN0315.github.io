using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.Controllers
{
    public class SellerInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ReviewSeller()
        {
            return View();
        }
    }
}
