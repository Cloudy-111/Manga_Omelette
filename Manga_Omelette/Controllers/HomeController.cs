using Manga_Omelette.Data;
using Manga_Omelette.Models;
using MangaASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Manga_Omelette.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Manga_OmeletteDBContext _db;
        public HomeController(Manga_OmeletteDBContext db, ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = db;
        }
        private IQueryable<Story> GetPopularStories()
        {
            var popular_stories = _db.Story.OrderByDescending(s => s.PopularPoint).ThenBy(s => s.Title).Take(10);
            return popular_stories;
        }

        public IActionResult Index()
        {
            IEnumerable<Story> popular_stories = GetPopularStories();
            return View(popular_stories);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Setting()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
