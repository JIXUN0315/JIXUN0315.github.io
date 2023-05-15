using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.Controllers
{
    public class BackNotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
