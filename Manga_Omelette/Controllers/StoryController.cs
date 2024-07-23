using Manga_Omelette.Data;
using MangaASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Linq;

using Manga_Omelette.Areas.Identity.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Services;
using Microsoft.EntityFrameworkCore;

namespace MangaASP.Controllers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomRouteAttribute : RouteAttribute
    {
        public CustomRouteAttribute(string template) : base(template)
        {
        }
    }
    //[Route("Story")]
    public class StoryController : Controller
    {
        private readonly Manga_OmeletteDBContext _db;
        private UserManager<User> _userManager;
        private readonly ChapterService _chapterServices;
        private readonly StoryService _storyService;
        public StoryController(Manga_OmeletteDBContext db, UserManager<User> userManager, ChapterService chapterService, StoryService storyService)
        {
            _db = db;
            _userManager = userManager;
			_chapterServices = chapterService;
            _storyService = storyService;
		}
        private IEnumerable<Story> GetStoriesForEachPage(int page)
        {
            IEnumerable<Story> stories = _db.Story;
            return stories;
        }
        //[CustomRoute("titles")]
        public IActionResult SearchView(int page = 1)
        {
            int items_per_page = 10;
            IQueryable<Story> storyList = _storyService.GetStoriesForEachPage(page, items_per_page);
            int totalStories = _db.Story.Count();
            int totalPages = (int)Math.Ceiling((double)totalStories / items_per_page);
            
            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;
            
            return View(storyList);
        }
        [Authorize]
        public IActionResult FollowList()
        {
            return View();
        }
        //[CustomRoute("latest-update")]
        public IActionResult Latest_Update(int page = 1)
        {
            int items_per_page = 10;
            IEnumerable<Story> storyList = GetStoriesForEachPage(page).OrderBy(s => s.UpdateDate).Skip((page - 1) * items_per_page).Take(items_per_page); ;
            int totalStories = _db.Story.Count();
            int totalPages = (int)Math.Ceiling((double)totalStories / items_per_page);

            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;

            return View(storyList);
        }
        //[CustomRoute("top-read")]
        public IActionResult Top_Read()
        {
            return View();
        }
        //[CustomRoute("top-like")]
        public IActionResult Top_Like()
        {
            return View();
        }
        //[CustomRoute("for-you")]
        public IActionResult For_You()
        {
            return View();
        }
        //[CustomRoute("random")]
        public IActionResult Random()
        {
            return View();
        }
        public IActionResult Details_Story(int id)
        {
			Story story = _storyService.getSingleStory(id);
            if (story == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            var isInFollowList = _db.FavoriteList.Any(f => f.UserId == userId && f.StoryId == id);
			var favorite = new FavoriteList
            {
                StoryId = id,
                UserId = userId
			};
            var ModelInDetails_Story = new DetailsStoryViewModel()
            {
                story = story,
                favorite = favorite,
                isInFavoriteList = isInFollowList,
                chapters = story.Chapters.OrderBy(c => c.Id).ToList(),
                ListComment = story.Comments,
                ListGenre = story.Story_Genres.Select(sg => sg.Genre).ToList()
            };
            ViewBag.TitlePage = story.Title;
            //ViewBag.Favorite = favorite;
            return View(ModelInDetails_Story);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToFollowList(FavoriteList obj)
        {
			if (obj != null)
            {
                _db.FavoriteList.Add(obj);
                _db.SaveChanges();
				TempData["success_add"] = "Story Add to Library Successfully!";
				return Json(new { success = true, message = "Story Added to Library Successfully!" });
			}
			TempData["failure_add"] = "Failed to Add Story to Library!";
			return Json(new { success = false, message = "Failed to Add Story to Library!" });
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromList(string userId, int storyId)
        {
            FavoriteList obj = _db.FavoriteList.FirstOrDefault(f => f.UserId == userId && f.StoryId == storyId);
            if(obj != null)
            {
                _db.FavoriteList.Remove(obj);
                _db.SaveChanges();
				TempData["success_remove"] = "Story Remove From List Successfully!";
				return Json(new { success = true, message = "Story Removed from Library Successfully!" });
			}
			TempData["fail_remove"] = "Failed to remove From Library!";
			return Json(new { success = false, message = "Failed to Remove Story from Library!" });
		}

	}
}
