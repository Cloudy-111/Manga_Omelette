using Manga_Omelette.Areas.Identity.Data;
using Manga_Omelette.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Models_Secondary;
using Manga_Omelette.Services;
using Manga_Omelette.SignalR;
using MangaASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Manga_Omelette.Controllers
{
    public class ChapterController : Controller
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly ChapterService _chapterService;
        private readonly CommentService _commentService;
        private readonly StoryService _storyService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly FavoriteService _favoriteService;
        private readonly NotificationService _notificationService;
        private readonly UserManager<User> _userManager;

        private readonly IHubContext<ChatHub> _hubContext;

        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };
        public ChapterController(
            Manga_OmeletteDBContext db, 
            ChapterService chapterService, 
            CommentService commentService, 
            StoryService storyService, 
            CloudinaryService cloudinaryService, 
            FavoriteService favoriteService, 
            NotificationService notificationService,
            IHubContext<ChatHub> hubContext,
            UserManager<User> userManager)
        {
            _db = db;
            _chapterService = chapterService;
            _commentService = commentService;
            _storyService = storyService;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
            _favoriteService = favoriteService;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index(int id)
        {
            Chapter obj = _chapterService.GetChapterById(id);
            if(obj == null)
            {
                return NotFound();
            }
            Story story = _chapterService.GetStoryByChapterId(id);
            var ChapterViewModel = new ChapterViewModel
            {
				CommentAmount = _commentService.getAmountOfComment(id),
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

        [HttpGet]
        //GetComments when click 'load-more-comment' - lazy loading, load comment has id greater last comment id
        //Now load all origin comment, then load all Replies.
        public IActionResult GetComments(int chapterId, int lastCommentId, int amount = 10)
        {
            var comments = _db.Comment
                .Where(c => c.ChapterId == chapterId && c.Id > lastCommentId && c.ParentCommentId == null)
                .OrderBy(c => c.Id)
                .Take(amount)
                //To load userNameDisplay(Additional attribute) in a comment, join table of user and comment
                //use Include and create Forward Model (View Model to pass information)
                .Include(c => c.User)
                .Select(c => new Comment_User
                {
                    Id = c.Id,
                    comment_content = c.Content,
                    CreatedDate = c.CreateDate.ToString(),
                    userId = c.UserId,
                    userNameDisplay = c.User.NameDisplay,
                    reply_amount = _commentService.getAmountOfReply(c.Id),
				})
                .ToList();
            return Json(comments);
        }

        [HttpGet]
        public IActionResult GetRepliesComment(int parentCommentId, int lastReplyId, int amount = 5)
        {
            var comments = _db.Comment
                .Where(c => c.ParentCommentId == parentCommentId && c.Id > lastReplyId)
                .OrderBy(c => c.Id)
                .Take(amount)
                .Include(c => c.User)
                .Select(c => new Comment_User
                {
                    Id = c.Id,
                    comment_content = c.Content,
                    CreatedDate = c.CreateDate.ToString(),
                    userId = c.UserId,
                    userNameDisplay = c.User.NameDisplay,
                })
                .ToList();
            return Json(comments);
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
        public async Task<IActionResult> CreateChapter(CreateChapterViewModel model)
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
            _favoriteService.UpdateWhenHasNewChapter(model.story.Id);

            //Create Notification for all user Who follow
            var titleStory = _storyService.getOnlyStory(model.chapter.StoryId).Title;
            var newNotification = new NotificationMongo()
            {
                Id = Guid.NewGuid().ToString(),
                Title = $"{titleStory}: New Chapter Has Been Upload",
                Content = "from ADMIN",
                CreateDate = DateTime.Now,
                TypeId = _notificationService.getTypeId("ADMIN"),
                SenderId = _userManager.GetUserId(User),
            };
			await _notificationService.CreateNotification(newNotification);

			var followsOfStory = _favoriteService.GetUserFollowOfEachStory(model.story.Id);
            var NotificationsForFollow = new List<Notification_UserMongo>();
            foreach(User user in followsOfStory)
            {
                NotificationsForFollow.Add(new Notification_UserMongo
				{
                    Id = Guid.NewGuid().ToString(),
                    NotificationId = newNotification.Id,
                    UserId = user.Id,
                });
            }
            await _notificationService.InsertManyAsync(NotificationsForFollow);

            var notificationViewModel = new NotificationViewModel
            {
                Id = newNotification.Id,
                Title = newNotification.Title,
                Content = newNotification.Content,
                CreateDate = newNotification.CreateDate,
                TypeId = newNotification.TypeId,
                SenderId = newNotification.SenderId
            };

            return Json(new
            {
                success = true,
                notification = notificationViewModel,
                redirectUrl = Url.Action("ManageStory", "Administration")
            });
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

        [Authorize(Roles = "Super ADMIN, ADMIN")]
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
