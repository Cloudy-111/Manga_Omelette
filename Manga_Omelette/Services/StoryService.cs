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
				//.OrderBy(s => s.Title)
				.Skip((page - 1) * items_per_page)
				.Take(items_per_page);
        }
		public IEnumerable<Story_Genre> ListGenreExistInStory(int storyId)
		{
			return _db.Story_Genre.Where(sg => sg.StoryId == storyId);
		}
		public void UpdateStoryRating(int storyId)
		{
			Story story = _db.Story.FirstOrDefault(s => s.Id == storyId);
			if(story != null)
			{
				var rating = _db.Rating.Where(r => r.StoryId == storyId).ToList();
				if (rating.Any())
				{
					story.Rate_Average = rating.Average(r => r.Score);
				}
				else
				{
					story.Rate_Average = 0;
				}
				_db.SaveChanges();
			}
		}

    }
}
