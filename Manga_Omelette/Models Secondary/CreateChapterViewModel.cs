using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class CreateChapterViewModel
    {
        public Chapter chapter {  get; set; }
        public Story story { get; set; }
        public List<IFormFile> imageFiles { get; set; }
        public List<string> imageURLs { get; set; } 
    }
}
