using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class StoryFilterViewModel
    {
        public string Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; }
        public List<Story> stories { get; set; }
    }
}
