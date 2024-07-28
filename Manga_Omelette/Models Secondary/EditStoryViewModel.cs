using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class EditStoryViewModel
    {
        public Story story { get; set; }
		public IFormFile? imageFile { get; set; }
        public List<Genre> ListGenre { get; set; }
        public List<Genre> AllGenre { get; set; }
		public string GenreIds { get; set; }
        public IEnumerable<Chapter> Chapters { get; set; }
	}
}
