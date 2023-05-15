using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
