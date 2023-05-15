using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
