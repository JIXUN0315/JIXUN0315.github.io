using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult UserName()
        {
            return View();
        }
    }
}
