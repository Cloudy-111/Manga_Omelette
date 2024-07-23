using Manga_Omelette.Data;
using MangaASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace Manga_Omelette.Controllers
{
	public class FavoriteController : Controller
	{
		private readonly Manga_OmeletteDBContext _db;
		public FavoriteController(Manga_OmeletteDBContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddToFollowList(FavoriteList obj)
		{
			if(ModelState.IsValid) { 
				_db.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Story Add to Library Successfully!";
				return RedirectToAction("Details_Story", "Story", new { id = obj.StoryId });
			}
			TempData["failure"] = "Failed to Add Story to Library!";
			return RedirectToAction("Details_Story", "Story", new { id = obj.StoryId });
		}
	}
}
