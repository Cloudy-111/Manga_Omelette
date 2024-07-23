using System.ComponentModel.DataAnnotations;

namespace MangaASP.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Genre Name can't be longer 20 letters!")]
        public string Name { get; set; }
        public ICollection<Story_Genre> StoriesGenre { get; set; }
    }
}
