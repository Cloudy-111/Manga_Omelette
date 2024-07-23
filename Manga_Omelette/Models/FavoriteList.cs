using Manga_Omelette.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaASP.Models
{
    public class FavoriteList
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Story")]
        public int StoryId { get; set; }
        public Story Story { get; set; }
    }
}
