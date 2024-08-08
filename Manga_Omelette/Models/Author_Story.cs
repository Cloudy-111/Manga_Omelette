using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaASP.Models
{
    public class Author_Story
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [ForeignKey("Story")]
        public int StoryId { get; set; }
        public Story Story { get; set; }
        public bool isArtist { get; set; }
		public bool isAuthor { get; set; }
	}
}
