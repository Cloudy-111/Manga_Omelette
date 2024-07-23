using Manga_Omelette.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaASP.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }
        [ForeignKey("Story")]
        public int StoryId { get; set; }

        public User User { get; set; }
        public Story Story { get; set; }
        public Chapter Chapter { get; set; }
        [Required]
        public string Content { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public IEnumerable<Comment> Replies { get; set; }
        public bool isDeleted { get; set; } = false;
	}
}
