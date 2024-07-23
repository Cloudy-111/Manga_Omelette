using System.ComponentModel.DataAnnotations;

namespace MangaASP.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public ICollection<Author_Story> Author_Stories { get; set; }
    }
}
