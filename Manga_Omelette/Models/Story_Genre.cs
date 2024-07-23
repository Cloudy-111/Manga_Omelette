using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaASP.Models
{
    public class Story_Genre
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Story")]
        public int StoryId { get; set; }
        public Story Story { get; set; }
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
