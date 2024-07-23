using MangaASP.Models;

namespace Manga_Omelette.Models
{
	public class ChapterViewModel
	{
		public IEnumerable<Comment> ListComment { get; set; }
		public Comment NewComment { get; set; }
		public Chapter Chapter { get; set; }
		public List<Chapter> Chapters { get; set; }
		public ICollection<ImageInChapter> ImageInChapter { get; set; }
		public Story Story { get; set; }
	}
}
