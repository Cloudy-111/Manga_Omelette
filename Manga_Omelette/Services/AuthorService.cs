using Manga_Omelette.Data;
using Manga_Omelette.Models_Secondary;

namespace Manga_Omelette.Services
{
    public class AuthorService
    {
        private readonly Manga_OmeletteDBContext _db;
        public AuthorService(Manga_OmeletteDBContext db)
        {
            _db = db;
        }
        public List<AuthorSearchResultViewModel> GetAuthorByTerm(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return null;
            }
            var results = _db.Author
                            .Where(au => au.Name.ToLower().Contains(term.ToLower()))
                            .Select(au => new AuthorSearchResultViewModel
                            {
                                Name = au.Name,
                                Id = au.Id,
                            })
                            .Take(5)
                            .ToList();
            return results;
        }

        public IEnumerable<AuthorSearchResultViewModel> GetAuthorLiveSearch(string search_author)
        {
            if (string.IsNullOrEmpty(search_author))
            {
                return null;
            }
            var results = _db.Author
                            .Where(au => au.Name.ToLower().Contains(search_author.ToLower()))
                            .Select(au => new AuthorSearchResultViewModel
                            {
                                Name = au.Name,
                                Id = au.Id,
                            })
                            .Take(10)
                            .ToList();
            return results;
        }
    }
}
