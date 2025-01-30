using Manga_Omelette.Services;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Manga_Omelette.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorApiController : Controller
    {
        private readonly AuthorService _authorService;

        public AuthorApiController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult GetAuthorLiveSearch(string name) // tên của query parameter trong URL phải khớp với tên tham số trong controller
        {
            var authors = _authorService.GetAuthorLiveSearch(name);
            return Ok(authors);
        }
    }
}
