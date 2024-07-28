using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class EditChapterViewModel
    {
        public Chapter chapter { get; set; }
        public List<IFormFile> imageFiles { get; set; }
    }
}
