using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaASP.Models
{
    public class ImageInChapter
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
    }
}
