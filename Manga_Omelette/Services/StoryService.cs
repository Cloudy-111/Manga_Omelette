using Manga_Omelette.Data;
using Manga_Omelette.Models_Secondary;
using MangaASP.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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
				.Include(s => s.Author_Stories)
				.ThenInclude(aus => aus.Author)
				.FirstOrDefault(story => story.Id == id);
			return result;
		}
		public Story getOnlyStory(int id)
		{
			Story result = _db.Story.FirstOrDefault(story => story.Id == id);
			return result;
		}
		public Story getStoryForCalPoint(int storyId)
		{
			Story result = _db.Story
				.Include(s => s.Comments)
				.FirstOrDefault(s => s.Id == storyId);
			return result;
		}
		public List<Genre> getGenreByStoryId(int id)
		{
			var story = _db.Story.Include(s => s.Story_Genres).ThenInclude(s => s.Genre).FirstOrDefault(s => s.Id == id);
			if (story != null)
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
		public IQueryable<Story> GetStoriesFollowEachPage(int page, int items_per_page, string userId)
		{
			return _db.Story
				.Include(s => s.FavoriteLists)
				.Where(s => s.FavoriteLists.Any(f => f.UserId == userId))
				.Skip((page - 1) * items_per_page)
				.Take(items_per_page);
		}
		public IQueryable<Story> GetStoriesFollow(string userId)
		{
			return _db.Story
				.Include(s => s.FavoriteLists)
				.Where(s => s.FavoriteLists.Any(f => f.UserId == userId));
        }

        public IEnumerable<Story_Genre> ListGenreExistInStory(int storyId)
		{
			return _db.Story_Genre.Where(sg => sg.StoryId == storyId);
		}
		public IEnumerable<Author_Story> ListAuthorExistInStory(int storyId)
		{
			return _db.Author_Story.Where(aus => aus.StoryId == storyId);
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
		public void AddGenreStory(string listGenreIds, int storyId)
		{
			var selectGenreIds = listGenreIds.Split(',').Select(id => int.Parse(id));
			var newGenreStories = new List<Story_Genre>();
			foreach (var genreId in selectGenreIds)
			{
				var newGenre_Story = new Story_Genre()
				{
					StoryId = storyId,
					GenreId = genreId,
				};
				newGenreStories.Add(newGenre_Story);
			}
			//Add genre in list genre of a story
			_db.Story_Genre.AddRange(newGenreStories);
		}
		public void AddAuthorStory(string listAuthorIds, string listArtistIds, int storyId)
		{
            var selectAuthorIds = new HashSet<int>(listAuthorIds.Split(',').Select(id => int.Parse(id)));
			var selectArtistIds = new HashSet<int>(listArtistIds.Split(',').Select(id => int.Parse(id)));
            
			var allIds = selectAuthorIds.Union(selectArtistIds).ToList();
			
			var newAuthorStories = new List<Author_Story>();
            foreach (var authorId in allIds)
            {
				bool isInAuthor = selectAuthorIds.Contains(authorId);
				bool isInArtist = selectArtistIds.Contains(authorId);

                var newAuthor_Story = new Author_Story()
                {
                    AuthorId = authorId,
                    StoryId = storyId,
                    isArtist = isInArtist,
                    isAuthor = isInAuthor,
                };
                newAuthorStories.Add(newAuthor_Story);
            }
            _db.Author_Story.AddRange(newAuthorStories);
        }

		public async Task<List<Author>> CreateAuthor(List<string>listNewAuthor)
		{
			var authors = new List<Author>();
			foreach(var name in listNewAuthor)
			{
				Author obj = new Author() { Name = name };
				authors.Add(obj);
			}
			_db.Author.AddRange(authors);
            await _db.SaveChangesAsync();

            var authorIds = authors.Select(a => a.Id).ToList();

            return authors;
		}
		public async Task CreateAuthorStory(string listNewAuthor, string listNewArtist, int storyId)
		{
			var selectAuthor = new HashSet<string>(listNewAuthor.Split(',').ToList());
			var selectArtist = new HashSet<string>(listNewArtist.Split(',').ToList());

			var allNewNames = selectAuthor.Union(selectArtist).ToList();

			var newAuthors = await CreateAuthor(allNewNames);
            var newAuthorStories = new List<Author_Story>();
            foreach (var author in newAuthors)
            {
                bool isInAuthor = selectAuthor.Contains(author.Name);
                bool isInArtist = selectArtist.Contains(author.Name);

                var newAuthor_Story = new Author_Story()
                {
                    AuthorId = author.Id,
                    StoryId = storyId,
                    isArtist = isInArtist,
                    isAuthor = isInAuthor,
                };
                newAuthorStories.Add(newAuthor_Story);
            }
            _db.Author_Story.AddRange(newAuthorStories);
        }
		public void UpdatePopularPoint(int storyId)
		{
			Story story = getStoryForCalPoint(storyId);
			var views = story.Views;
			var shares = story.Shares;
			var rating = story.Rate_Average;
			var comments = story.Comments.Count();
			var likes = _db.FavoriteList.Count(fl => fl.StoryId == storyId);
			story.PopularPoint = 
				(views > 0 ? Math.Log10(views) : 0) +
				(shares > 0 ? Math.Log10(shares) : 0) +
				(comments > 0 ? Math.Log10(comments) : 0) +
				(likes > 0 ? Math.Log10(likes) : 0) + 
				rating
				;
		}

		public List<StoriesSearchResultViewModel> GetStoryByTerm(string term)
		{
			if (string.IsNullOrEmpty(term))
			{
				return null;
			}
			var results = _db.Story
							.Where(s => s.Title.ToLower().Contains(term.ToLower()))
							.Select(s => new StoriesSearchResultViewModel
							{
								Title = s.Title,
								Id = s.Id
							})
							.Take(5)
							.ToList();
			return results;
		}
    }
}
