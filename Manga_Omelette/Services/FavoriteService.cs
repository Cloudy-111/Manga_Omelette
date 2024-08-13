using Manga_Omelette.Data;
using MangaASP.Models;
using Microsoft.EntityFrameworkCore;

namespace Manga_Omelette.Services
{
    public class FavoriteService
    {
        private readonly Manga_OmeletteDBContext _db;
        public FavoriteService(Manga_OmeletteDBContext db)
        {
            _db = db;
        }
        public void UpdateWhenHasNewChapter(int storyId)
        {
            _db.Database.ExecuteSqlRaw("UPDATE FavoriteList SET NewUpdate = 1 WHERE StoryId = {0}", storyId);
        }
        public void RemoveUpdateNewChapter(int storyId, string userId)
        {
            _db.Database.ExecuteSqlRaw("UPDATE FavoriteList SET NewUpdate = 0 Where StoryId = {0} AND UserId = {1}", storyId, userId);
        }
    }
}
