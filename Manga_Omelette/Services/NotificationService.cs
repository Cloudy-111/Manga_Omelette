using Manga_Omelette.Data;
using Manga_Omelette.Models;

namespace Manga_Omelette.Services
{
    public class NotificationService
    {
        private readonly Manga_OmeletteDBContext _db;

        public NotificationService(Manga_OmeletteDBContext db)
        {
            _db = db;
        }

        public IQueryable<Notification> GetNotificationForEachPage(int page, int items_per_page, string typeId)
        {
            return _db.Notification
                .Where(n => n.TypeId == typeId)
                .OrderBy(n => n.CreateDate)
                .Skip((page - 1) * items_per_page)
                .Take(items_per_page);
        }

        public string getTypeId(string typeName)
        {
            return _db.TypeNotis.FirstOrDefault(tn => tn.Name == typeName).Id;
        }
        public IQueryable<Notification> GetSystemNotification(string typeName, int page, int items_per_page)
        {
            return _db.Notification.Where(n => n.TypeNotis.Name.ToLower() == typeName.ToLower())
                .OrderBy(n => n.CreateDate)
                .Skip((page - 1) * items_per_page)
                .Take(items_per_page);
        }

        public IQueryable<Notification> GetAdminNotification(string userId, int page, int items_per_page)
        {
            return _db.Notification.Where(n => n.Notification_User.Any(n_u => n_u.UserId == userId))
                .OrderBy(n => n.CreateDate)
                .Skip((page - 1) * items_per_page)
                .Take(items_per_page);
        }
    }
}
