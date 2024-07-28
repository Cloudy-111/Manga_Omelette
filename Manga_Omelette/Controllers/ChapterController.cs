using Manga_Omelette.Areas.Identity.Data;
using Manga_Omelette.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Models_Secondary;
using Manga_Omelette.Services;
using MangaASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Manga_Omelette.Controllers
{
    public class ChapterController : Controller
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly ChapterService _chapterService;
        private readonly CommentService _commentService;
        private readonly StoryService _storyService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly UserManager<User> _userManager;

        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };
        public ChapterController(Manga_OmeletteDBContext db, ChapterService chapterService, CommentService commentService, StoryService storyService, CloudinaryService cloudinaryService, UserManager<User> userManager)
        {
            _db = db;
            _chapterService = chapterService;
            _commentService = commentService;
            _storyService = storyService;
            _cloudinaryService = cloudinaryService;
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

        [Authorize(Roles = "Super ADMIN, ADMIN")]
        public IActionResult CreateChapter(int storyId)
        {
            var story = _storyService.getSingleStory(storyId);
            var CreateChapterViewModel = new CreateChapterViewModel()
            {
                story = story,
                chapter = new Chapter()
                {
                    StoryId = storyId,
                },
                imageFiles = new List<IFormFile>(),
            };
            return View(CreateChapterViewModel);
        }

        [Authorize(Roles = "Super ADMIN, ADMIN")]
        [HttpPost]
        public IActionResult CreateChapter(CreateChapterViewModel model)
        {
            if(model == null)
            {
                return BadRequest("No File Choosen!");
            }
            var uploadImageResult = new List<string>();
            foreach(var image in model.imageFiles)
            {
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if(string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    return BadRequest("Invalid File type!");
                }
                try
                {
                    var uploadResult = _cloudinaryService.UploadImage(image);
                    uploadImageResult.Add(uploadResult);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, $"Error uploading file {image.FileName}: {e.Message}");
                }
            }

            model.chapter.StoryId = model.story.Id;
            model.chapter.UpdateDate = DateTime.Now;

            _db.Add(model.chapter);
            _db.SaveChanges();

            var listImageChapter = new List<ImageInChapter>();
            foreach(var image in uploadImageResult)
            {
                var ImageInChapter = new ImageInChapter()
                {
                    ChapterId = model.chapter.Id,
                    Path = image,
                    Filename = Path.GetFileName(image),
                };
                listImageChapter.Add(ImageInChapter);
            }

            _db.ImageInChapter.AddRange(listImageChapter);
            _db.SaveChanges();

            return RedirectToAction("ManageStory", "Administration");
        }

        [Authorize(Roles = "Super ADMIN, ADMIN")]
        public IActionResult EditChapter(int chapterId)
        {
            Chapter chapter = _chapterService.GetChapterById(chapterId);
            var EditChapterViewModel = new EditChapterViewModel()
            {
                chapter = chapter,
                imageFiles = new List<IFormFile>(),
            };
            return View(EditChapterViewModel);
        }

        [HttpPost]
        public IActionResult EditChapter(EditChapterViewModel model)
        {
            if(model == null)
            {
                return BadRequest(ModelState);
            }
            var chapter = _chapterService.GetChapterById(model.chapter.Id);
            var uploadImageResult = new List<string>();
            foreach(var image in model.imageFiles)
            {
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if(string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    return BadRequest("Invalid File Type!");
                }
                try
                {
                    var uploadResult = _cloudinaryService.UploadImage(image);
                    uploadImageResult.Add(uploadResult);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error uploading file {image.FileName}: {ex.Message}");
                }
            }

            //Delete Image on Cloudinary
            foreach (var imageChapter in chapter.imageInChapters)
            {
                _cloudinaryService.DeleteImage(imageChapter.Path);
            }


            chapter.UpdateDate = DateTime.Now;
            _db.Chapter.Update(chapter);

            //Delete old Images in DB
            var removeListImageInChapter = _db.ImageInChapter.Where(i => i.ChapterId == model.chapter.Id);
            
            _db.RemoveRange(removeListImageInChapter);

            //Add images in DB
            var listImageChapter = new List<ImageInChapter>();
            foreach (var image in uploadImageResult)
            {
                var ImageInChapter = new ImageInChapter()
                {
                    ChapterId = model.chapter.Id,
                    Path = image,
                    Filename = Path.GetFileName(image),
                };
                listImageChapter.Add(ImageInChapter);
            }
            _db.ImageInChapter.AddRange(listImageChapter);
            

            _db.SaveChanges();

            
            return RedirectToAction("ManageStory", "Administration");
        }
    }
}
