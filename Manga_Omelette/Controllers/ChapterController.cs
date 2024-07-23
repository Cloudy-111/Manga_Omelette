using Manga_Omelette.Areas.Identity.Data;
using Manga_Omelette.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Services;
using MangaASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Manga_Omelette.Controllers
{
    public class ChapterController : Controller
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly ChapterService _chapterService;
        private readonly CommentService _commentService;
        private readonly UserManager<User> _userManager;
        public ChapterController(Manga_OmeletteDBContext db, ChapterService chapterService, CommentService commentService, UserManager<User> userManager)
        {
            _db = db;
            _chapterService = chapterService;
            _commentService = commentService;
            _userManager = userManager;
        }
        public IActionResult Index(int id)
        {
            Chapter obj = _chapterService.GetChapterById(id);
            if(obj == null)
            {
                return NotFound();
            }
            Story story = _chapterService.GetStoryByChapterId(id);
            var ChapterViewModel = new ChapterViewModel
            {
                ListComment = obj.Comments,
                NewComment = new Comment
                {
                    UserId = _userManager.GetUserId(User),
                    ChapterId = id,
                    StoryId = _chapterService.GetStoryByChapterId(id).Id,
                },
                Chapter = obj,
                Story = story,
                ImageInChapter = obj.imageInChapters,
                Chapters = story.Chapters.OrderBy(c => c.Id).ToList()
            };
            return View(ChapterViewModel);
        }
    }
}
