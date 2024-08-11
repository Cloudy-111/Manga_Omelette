using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MangaASP.Models
{
    public class Story
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string CoverImage { get; set; }
        public double Rate_Average { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Views { get; set; } = 0;
        public int Shares { get; set; } = 0;
        public double PopularPoint { get; set; } = 0;
        public ICollection<Author_Story> Author_Stories { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
        public ICollection<Story_Genre> Story_Genres { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Rating { get; set; }
        public ICollection<FavoriteList> FavoriteLists { get; set; }

    }
}
