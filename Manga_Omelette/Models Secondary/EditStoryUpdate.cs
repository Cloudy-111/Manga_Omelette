using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
	public class EditStoryUpdate
	{
		public Story story { get; set; }
		public IFormFile? imageFile { get; set; }
		public string GenreIds { get; set; }
	}
}
