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
		public Comment GetCommentById(int cmtId)
		{
			return _db.Comment.Include(c => c.Replies).FirstOrDefault(c => c.Id == cmtId);
		}
		public List<Comment> GetCommentsByChapterId(int chapterId, int lastCommentId, int amount)
		{
			return _db.Comment
				.Where(c => c.ChapterId == chapterId && c.Id > lastCommentId)
				.OrderBy(c => c.Id)
				.Take(amount)
				.ToList();
		}
		public int getAmountOfComment(int chapterId)
		{
			return _db.Comment.Count(cmt => cmt.ChapterId == chapterId);
		}
		public int getAmountOfReply(int commentId)
		{
			return _db.Comment.Count(cmt => cmt.ParentCommentId == commentId);
		}
	}
}
