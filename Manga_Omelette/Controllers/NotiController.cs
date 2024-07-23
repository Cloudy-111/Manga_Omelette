using Microsoft.AspNetCore.Mvc;

namespace MangaASP.Controllers
{
    public class NotiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
