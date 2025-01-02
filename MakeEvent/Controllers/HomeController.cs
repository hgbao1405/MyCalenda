using Microsoft.AspNetCore.Mvc;

namespace MakeEvent.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
