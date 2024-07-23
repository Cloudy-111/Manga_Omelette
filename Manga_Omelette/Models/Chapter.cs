using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaASP.Models
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("Story")]
        public int StoryId { get; set; }
        public Story Story { get; set; } 
        public ICollection<ImageInChapter> imageInChapters { get; set; }
        public ICollection<Comment> Comments { get; set; } 
    }
}
