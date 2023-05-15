using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.Controllers
{
    public class BannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
