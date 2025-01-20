using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class StoryFilterResult
    {
        public List<Story> Stories { get; set; }
        public int TotalCount { get; set; }
    }
}
