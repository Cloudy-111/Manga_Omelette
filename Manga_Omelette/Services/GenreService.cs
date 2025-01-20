using Manga_Omelette.Data;
using MangaASP.Models;

namespace Manga_Omelette.Services
{
    public class GenreService
    {
        private readonly Manga_OmeletteDBContext _db;
        public GenreService(Manga_OmeletteDBContext db)
        {
            _db = db;
        }
        public List<Genre> getAllGenre()
        {
            return _db.Genre.ToList();
        }
    }
}
