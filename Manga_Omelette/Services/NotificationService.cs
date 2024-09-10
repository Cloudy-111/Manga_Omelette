using Manga_Omelette.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Models_Secondary;
using MongoDB.Driver;

namespace Manga_Omelette.Services
{
    public class NotificationService
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly IMongoCollection<NotificationMongo> _notificationsMongo;

        public NotificationService(Manga_OmeletteDBContext db, IMongoDatabase mdb)
        {
            _db = db;
            _notificationsMongo = mdb.GetCollection<NotificationMongo>("Notifications");
        }

        //Add 1 Notification in MongoDB
        public Task CreateNotification(NotificationMongo notificationMongo)
        {
            return _notificationsMongo.InsertOneAsync(notificationMongo);
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
				.OrderByDescending(n => n.CreateDate)
                .Skip((page - 1) * items_per_page)
                .Take(items_per_page);
        }

        public IQueryable<Notification> GetAdminNotification(string userId, int page, int items_per_page)
        {
            return _db.Notification.Where(n => n.Notification_User.Any(n_u => n_u.UserId == userId))
				.OrderByDescending(n => n.CreateDate)
                .Skip((page - 1) * items_per_page)
                .Take(items_per_page);
        }

        //Get 1 Notification By Id
        public async Task<NotificationMongo> GetSingleNotification(string notificationId)
        {
            return await _notificationsMongo.Find(notification => notification.Id == notificationId).FirstOrDefaultAsync();
        }
    }
}
