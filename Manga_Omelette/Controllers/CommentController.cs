using Manga_Omelette.Areas.Identity.Data;
using Manga_Omelette.Data;
using Manga_Omelette.Services;
using MangaASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Manga_Omelette.Controllers
{
	public class CommentController : Controller
	{
		private readonly Manga_OmeletteDBContext _db;
		private readonly CommentService _commentService;
		private readonly UserManager<User> _userManager;
		public CommentController(Manga_OmeletteDBContext db, CommentService commentService, UserManager<User> userManager)
		{
			_db = db;
			_userManager = userManager;
			_commentService = commentService;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult PostComment(Comment obj)
		{
			if(obj != null)
			{
				obj.CreateDate = DateTime.Now;
				_db.Add(obj);
				_db.SaveChanges();
				return Json(new {success = true, commentId = obj.Id});
			}
			//TempData["CheckObj"] = obj.Content;
			return Json(new { success = false});
		}
		//If Comment has Replies, Change content of Comment
		[HttpPost]
		public IActionResult DeleteComment(int commentId)
		{
			var obj = _commentService.GetCommentById(commentId);
			if(obj != null)
			{
				if(obj.Replies.Any()) {
					obj.Content = "This Comment has been Deleted!";
					obj.isDeleted = true;
					_db.SaveChanges();
					return Json(new { success = true });
				}
				else
				{
					_db.Remove(obj);
					_db.SaveChanges();
					return Json(new { success = true });
				}
			}
			return Json(new { success = false, message = "Comment not found!" });
		}
		//Get User By Id
		[HttpGet]
		public async Task<IActionResult> GetUserName(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			return Json(new { userName = user?.NameDisplay });
		}
	}
}
