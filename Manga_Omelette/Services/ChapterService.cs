using Manga_Omelette.Data;
using MangaASP.Models;
using Microsoft.EntityFrameworkCore;

namespace Manga_Omelette.Services
{
    public class ChapterService
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly StoryService _storyService;
        public ChapterService(Manga_OmeletteDBContext db, StoryService storyService)
        {
            _db = db;
            _storyService = storyService;
        }

        public IEnumerable<Chapter> GetListChapterByStoryId(int storyId)
        {
            IEnumerable<Chapter> result = _db.Chapter.Where(c => c.StoryId == storyId);
            return result;
        }

        //not load all comment in an object Chapter, load comment when call
        public Chapter GetChapterById(int id)
        {
            Chapter result = _db.Chapter
                .Include(c => c.imageInChapters)
                .FirstOrDefault(c => c.Id == id);
            return result;
        }
        public int GetStoryIdByChapterId(int id)
        {
            int result = _db.Chapter.FirstOrDefault(c => c.Id == id).StoryId;
            return result;
        }
		public Story GetStoryByChapterId(int id)
		{
			int storyId = _db.Chapter.FirstOrDefault(c => c.Id == id).StoryId;
            Story result = _storyService.getSingleStory(storyId);
			return result;
		}
	}
}
