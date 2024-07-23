using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaASP.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Story")]
        public int StoryId { get; set; }
        [Required]
        [Range(1, 10)]
        public int Score { get; set; }
    }
}
