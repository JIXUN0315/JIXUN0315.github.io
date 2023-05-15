using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
