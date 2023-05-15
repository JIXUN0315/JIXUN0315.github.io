using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.ViewComponents
{
    public class LoginViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
