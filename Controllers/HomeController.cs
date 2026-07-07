using Microsoft.AspNetCore.Mvc;

namespace diploma_strava_ex_api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
