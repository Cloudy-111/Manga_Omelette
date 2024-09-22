using Manga_Omelette.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Models_Secondary;
using Manga_Omelette.Services;
using MangaASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Manga_Omelette.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Manga_OmeletteDBContext _db;
        private readonly StoryService _storyService;
        private readonly AuthorService _authorService;
        public HomeController(
            Manga_OmeletteDBContext db, 
            ILogger<HomeController> logger, 
            StoryService storyService,
            AuthorService authorService)
        {
            _logger = logger;
            _db = db;
            _storyService = storyService;
            _authorService = authorService;
        }
        private IQueryable<Story> GetPopularStories()
        {
            var popular_stories = _db.Story
                .OrderByDescending(s => s.PopularPoint)
                .ThenBy(s => s.Title)
                .Include(s => s.Story_Genres)
                .ThenInclude(sg => sg.Genre)
                .Include(s => s.Author_Stories)
                .ThenInclude(aus => aus.Author)
                .Take(10);
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

        [HttpGet]
        public IActionResult Search(string term)
        {
            var results = new SearchResultPopupViewModel
            {
                storiesSearchResult = new List<StoriesSearchResultViewModel>(),
                authorSearchResult = new List<AuthorSearchResultViewModel>()
            };
            var story_results = _storyService.GetStoryByTerm(term);
            var author_results = _authorService.GetAuthorByTerm(term);
            if (!string.IsNullOrEmpty(term))
            {
                results.storiesSearchResult = story_results;
                results.authorSearchResult = author_results;
            };
            return Json(results);
        }
    }
}
