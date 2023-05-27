using Microsoft.AspNetCore.Mvc;

namespace shukersal_backend.ServiceLayer
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
