using Manga_Omelette.Data;
using MangaASP.Models;
using Microsoft.EntityFrameworkCore;

namespace Manga_Omelette.Services
{
	public class StoryService
	{
		private readonly Manga_OmeletteDBContext _db;
		public StoryService(Manga_OmeletteDBContext db)
		{
			_db = db;
		}
		public Story getSingleStory(int id)
		{
			Story result = _db.Story
				.Include(s => s.Chapters)
				.Include(s => s.Comments)
				.Include(s => s.Story_Genres)
				.ThenInclude(sg => sg.Genre)
				.FirstOrDefault(story => story.Id == id);
			return result;
		}
		public List<Genre> getGenreByStoryId(int id)
		{
			var story = _db.Story.Include(s => s.Story_Genres).ThenInclude(s => s.Genre).FirstOrDefault(s => s.Id == id);
			if(story != null)
			{
				return story.Story_Genres.Select(sg => sg.Genre).ToList();
			}
			return new List<Genre>();
		}
		public IQueryable<Story> GetStoriesForEachPage(int page, int items_per_page)
		{
			return _db.Story
				.OrderBy(s => s.Title)
				.Skip((page - 1) * items_per_page)
				.Take(items_per_page);
        }

    }
}
