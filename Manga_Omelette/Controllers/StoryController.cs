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
using Manga_Omelette.Models_Secondary;
using Microsoft.IdentityModel.Tokens;
using Sprache;

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
        private readonly CloudinaryService _cloudinaryService;

		private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
		public StoryController(Manga_OmeletteDBContext db, UserManager<User> userManager, ChapterService chapterService, StoryService storyService, CloudinaryService cloudinaryService)
        {
            _db = db;
            _userManager = userManager;
			_chapterServices = chapterService;
            _storyService = storyService;
            _cloudinaryService = cloudinaryService;
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
            IQueryable<Story> storyList = _storyService.GetStoriesForEachPage(page, items_per_page).OrderBy(s => s.Title);
            int totalStories = _db.Story.Count();
            int totalPages = (int)Math.Ceiling((double)totalStories / items_per_page);
            
            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;
            
            return View(storyList);
        }
        [Authorize]
        public IActionResult FollowList(int page = 1)
        {
			int items_per_page = 10;
			IQueryable<Story> storyList = _storyService.GetStoriesFollowEachPage(page, items_per_page, _userManager.GetUserId(User));
            int totalStories = storyList.Count();
            int totalPages = (int)Math.Ceiling((double)totalStories / items_per_page);

            return View(storyList);
        }
        //[CustomRoute("latest-update")]
        public IActionResult Latest_Update(int page = 1)
        {
            int items_per_page = 10;
            IEnumerable<Story> storyList = _storyService.GetStoriesForEachPage(page, items_per_page).OrderBy(s => s.UpdateDate);
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
			var rating = _db.Rating.Where(r => r.UserId == userId && r.StoryId == id).Select(r => r.Score).FirstOrDefault();
			var favorite = new FavoriteList
            {
                StoryId = id,
                UserId = userId
			};
			story.Views += 1;
			_storyService.UpdatePopularPoint(id);
			_db.SaveChanges();
			var ModelInDetails_Story = new DetailsStoryViewModel()
			{
				story = story,
				favorite = favorite,
				isInFavoriteList = isInFollowList,
				rating = rating,
				chapters = story.Chapters.OrderBy(c => c.Id).ToList(),
				ListComment = story.Comments,
				ListGenre = story.Story_Genres.Select(sg => sg.Genre).ToList(),
                ListAuthor = story.Author_Stories.Select(aus => new AuthorWithRoles
                {
                    Name = aus.Author.Name,
                    isArtist = aus.isArtist,
                    isAuthor = aus.isAuthor,
                }).ToList(),
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
				_storyService.UpdatePopularPoint(obj.StoryId);
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
				_storyService.UpdatePopularPoint(storyId);
				_db.SaveChanges();
				TempData["success_remove"] = "Story Remove From List Successfully!";
				return Json(new { success = true, message = "Story Removed from Library Successfully!" });
			}
			TempData["fail_remove"] = "Failed to remove From Library!";
			return Json(new { success = false, message = "Failed to Remove Story from Library!" });
		}

		[HttpPost]
		public IActionResult RateStory(string userId, int storyId, int rating_point)
		{
			if(!string.IsNullOrEmpty(userId) && storyId > 0 && rating_point > 0)
			{
				Rating obj = new Rating()
				{
					UserId = userId,
					StoryId = storyId,
					Score = rating_point
				};
				_db.Add(obj);
				_storyService.UpdatePopularPoint(storyId);
				_db.SaveChanges();
				_storyService.UpdateStoryRating(storyId);
				return Json(new { success = true });
			}
			return Json(new { success = false, userid = userId, storyId = storyId, rating_point = rating_point });
		}

		[HttpPost]
		public IActionResult EditRateStory(string userId, int storyId, int rating_point)
		{
			if (!string.IsNullOrEmpty(userId) && storyId > 0 && rating_point > 0)
			{
				Rating obj = _db.Rating.FirstOrDefault(r => r.UserId == userId && r.StoryId == storyId);
				obj.Score = rating_point;
				_db.Rating.Update(obj);
				_storyService.UpdatePopularPoint(storyId);
				_db.SaveChanges();
				_storyService.UpdateStoryRating(storyId);
				return Json(new { success = true });
			}
			return Json(new { success = false, userid = userId, storyId = storyId, rating_point = rating_point });
		}

		[HttpDelete]
		public IActionResult DeleteRateStory(string userId, int storyId)
		{
			if (!string.IsNullOrEmpty(userId) && storyId > 0)
			{
				Rating obj = _db.Rating.FirstOrDefault(r => r.UserId == userId && r.StoryId == storyId);
				_db.Remove(obj);
				_storyService.UpdatePopularPoint(storyId);
				_db.SaveChanges();
				_storyService.UpdateStoryRating(storyId);
				return Json(new { success = true });
			}
			return Json(new { success = false, userid = userId, storyId = storyId});
		}

		[HttpPost]
		public IActionResult IncreaseShares(int storyId)
		{
			Story story = _storyService.getOnlyStory(storyId);
			story.Shares += 1;
			_storyService.UpdatePopularPoint(storyId);
			_db.SaveChanges();
			return Json(new { success = true, message = "Copied to clipboard!" });
		}

		[Authorize(Roles = "Super ADMIN, ADMIN")]
		//==============================================Page CreateStory==============================================
		[HttpGet]
		public IActionResult CreateStory()
		{
			var model = new CreateStoryViewModel()
			{
				story = new Story(),
				imageFile = null,
				GenreIds = string.Empty,
				AllGenre = _db.Genre.ToList(),
				AllAuthor = _db.Author.ToList(),
			};
			return View(model);
		}

		[Authorize(Roles = "Super ADMIN, ADMIN")]
		//==============================================Post Create Story==============================================
		[HttpPost]
		public async Task<IActionResult> CreateStory(CreateStoryViewModel model)
		{
			if (model.imageFile == null)
			{
				TempData["No Choosen Image"] = "Please choose 1 Image in your device!";
				return RedirectToAction("CreateStory", "Story");
			}
			var ext = Path.GetExtension(model.imageFile.FileName).ToLowerInvariant();
			if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
			{
                TempData["Invalid File Type"] = "Invalid file type!";
                return RedirectToAction("CreateStory", "Story");
            }
			try
			{
                var uploadResultURL = _cloudinaryService.UploadImage(model.imageFile);
				model.story.CoverImage = uploadResultURL;
				model.story.UpdateDate = DateTime.Now;
				_db.Add(model.story);
				_db.SaveChanges();

				var listAuthorIds = Request.Form["ListAuthorIds"].ToString();
				var listNewAuthor = Request.Form["ListNewAuthor"].ToString();

				var listArtistIds = Request.Form["ListArtistIds"].ToString();
				var listNewArtist = Request.Form["ListNewArtist"].ToString();

				if (listAuthorIds.Length == 0 && listNewAuthor.Length == 0 && listArtistIds.Length == 0 && listNewArtist.Length == 0) { return NotFound(); }
				else
				{
					if (listAuthorIds.Length != 0 || listArtistIds.Length != 0) { _storyService.AddAuthorStory(listAuthorIds, listArtistIds, model.story.Id); }
                    if (listNewAuthor.Length != 0 || listNewArtist.Length != 0) { await _storyService.CreateAuthorStory(listNewAuthor, listNewArtist, model.story.Id); }
                }

				var listGenreIds = Request.Form["ListGenreIds"].ToString();
                if (listGenreIds.Length == 0) return NotFound();

				_storyService.AddGenreStory(listGenreIds, model.story.Id);
				_db.SaveChanges();

                TempData["Success Create Story"] = "Create Story Success";
                return RedirectToAction("ManageStory", "Administration");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(model);
		}

		[Authorize(Roles = "Super ADMIN")]
		//==============================================Page Edit Story==============================================
		[HttpGet]
		public IActionResult EditStory(int id)
		{
			Story story = _storyService.getSingleStory(id);
			var model = new EditStoryViewModel()
			{
				story = story,
				imageFile = null,
				ListGenre = story.Story_Genres.Select(sg => sg.Genre).ToList(),
				AllGenre = _db.Genre.ToList(),
				Chapters = story.Chapters.OrderBy(c => c.Id).ToList(),
				ListAuthor = story.Author_Stories.Select(aus => new AuthorWithRoles
				{
					AuthorId = aus.AuthorId,
					Name = aus.Author.Name,
					isArtist = aus.isArtist,
					isAuthor = aus.isAuthor,
				}).ToList(),
				AllAuthor = _db.Author.ToList(),
			};
			return View(model);
		}

		[Authorize(Roles = "Super ADMIN")]
		//==============================================Post Edit Story==============================================
		[HttpPost]
		public async Task<IActionResult> EditStory(EditStoryViewModel model)
		{
			if (model == null)
			{
				return NotFound();
			}
			Story story = _storyService.getSingleStory(model.story.Id);
			if (story == null)
			{
				return NotFound();

			}
			string storyImage = story.CoverImage;

			var existAuthor = _storyService.ListAuthorExistInStory(model.story.Id);
			_db.RemoveRange(existAuthor);

			var listAuthorIds = Request.Form["ListAuthorIds"].ToString();
			var listNewAuthor = Request.Form["ListNewAuthor"].ToString();

			var listArtistIds = Request.Form["ListArtistIds"].ToString();
			var listNewArtist = Request.Form["ListNewArtist"].ToString();

			if (listAuthorIds.Length == 0 && listNewAuthor.Length == 0 && listArtistIds.Length == 0 && listNewArtist.Length == 0) { return NotFound(); }
			else
			{
				if (listAuthorIds.Length != 0 || listArtistIds.Length != 0) { _storyService.AddAuthorStory(listAuthorIds, listArtistIds, model.story.Id); }
				if (listNewAuthor.Length != 0 || listNewArtist.Length != 0) { await _storyService.CreateAuthorStory(listNewAuthor, listNewArtist, model.story.Id); }
			}

			//If genre exist in list of genre of a story, Remove
			//then add them again
			var existGenre = _storyService.ListGenreExistInStory(model.story.Id);
			_db.RemoveRange(existGenre);

			//Take String ListGenreIds From form post in View
			var listGenreIds = Request.Form["ListGenreIds"].ToString();
			if (listGenreIds.Length == 0) return NotFound();

			_storyService.AddGenreStory(listGenreIds, model.story.Id);

			//Check if Admin has uploaded image
			Story storyAlter = _storyService.getSingleStory(model.story.Id);
			if (model.imageFile != null)
			{
				var ext = Path.GetExtension(model.imageFile.FileName).ToLower();
				if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
				{
					return BadRequest("Invalid File Type!");
				}
				try
				{
					//If Upload image, delete old one by URL
					var imageURL = _cloudinaryService.UploadImage(model.imageFile);
					storyAlter.CoverImage = imageURL;
					_cloudinaryService.DeleteImage(storyImage);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			//Update Information of Story
			storyAlter.Title = model.story.Title;
			storyAlter.Description = model.story.Description;
			storyAlter.UpdateDate = DateTime.Now;
			_db.Story.Update(storyAlter);
			_db.SaveChanges();

			//Success then go again Page with new Model
			var modelView = new EditStoryViewModel()
			{
				story = _storyService.getSingleStory(model.story.Id),
				imageFile = null,
				ListGenre = story.Story_Genres.Select(sg => sg.Genre).ToList(),
				AllGenre = _db.Genre.ToList(),
				Chapters = story.Chapters.OrderBy(c => c.Id).ToList(),
				ListAuthor = story.Author_Stories.Select(aus => new AuthorWithRoles
				{
					Name = aus.Author.Name,
					isArtist = aus.isArtist,
					isAuthor = aus.isAuthor,
				}).ToList(),
				AllAuthor = _db.Author.ToList(),
			};
			return View(modelView);
		}
	}
}
