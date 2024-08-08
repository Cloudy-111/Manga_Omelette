using Manga_Omelette.Models_Secondary;
using MangaASP.Models;

namespace Manga_Omelette.Models
{
	public class DetailsStoryViewModel
	{
		public Story story { get; set; }
		public FavoriteList favorite {  get; set; }
		public bool isInFavoriteList { get; set; }
		public int rating { get; set; } = 0;
		public IEnumerable<Chapter> chapters { get; set; }
		public IEnumerable<Comment> ListComment { get; set; }
		public List<Genre> ListGenre { get; set; }
        public List<AuthorWithRoles> ListAuthor { get; set; }
    }
}
