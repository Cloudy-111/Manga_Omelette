using Manga_Omelette.Data;
using MangaASP.Models;
using Microsoft.EntityFrameworkCore;

namespace Manga_Omelette.Services
{
	public class CommentService
	{
		private readonly Manga_OmeletteDBContext _db;
		public CommentService(Manga_OmeletteDBContext db)
		{
			_db = db;
		}
		public IEnumerable<Comment> GetCommentsByChapterId(int chapterId)
		{
			IEnumerable<Comment> result = _db.Comment.Include(cmt => cmt.Replies).Where(c => c.ChapterId == chapterId);
			return result;
		}
	}
}
