using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class StoryFilterViewModel
    {
        public string Keyword { get; set; }
        public List<int> IncludeGenres { get; set; } = new List<int>();
        public List<int> ExcludeGenres { get; set; } = new List<int>();
        public List<Genre> Genres { get; set; }
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; }
        public List<Story> stories { get; set; }
        public List<Author> authors { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Keyword) &&
                   !IncludeGenres.Any() &&
                   !ExcludeGenres.Any();
        }
    }
}
