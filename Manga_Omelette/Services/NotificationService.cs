using Manga_Omelette.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Models_Secondary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Manga_Omelette.Services
{
    public class NotificationService
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly IMongoCollection<NotificationMongo> _notificationsMongo;
        private readonly IMongoCollection<Notification_UserMongo> _notification_userMongo;

        public NotificationService(Manga_OmeletteDBContext db, IMongoDatabase mdb)
        {
            _db = db;
            _notificationsMongo = mdb.GetCollection<NotificationMongo>("Notifications");
            _notification_userMongo = mdb.GetCollection <Notification_UserMongo>("Notification_User");
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


        public async Task<List<NotificationMongo>> GetAdminNotificationByUseridAsync(string userId, int page, int items_per_page)
        {
			var user = Builders<Notification_UserMongo>.Filter.Eq(n_u => n_u.UserId, userId);
            var userNotification = await _notification_userMongo.Find(user).ToListAsync();

            var notiIds = userNotification.Select(n_u => n_u.NotificationId).ToList();

            var filterNotis = Builders<NotificationMongo>.Filter.In(n => n.Id, notiIds);
            var notifications = await _notificationsMongo
                                        .Find(filterNotis)
                                        .SortByDescending(n => n.CreateDate)
										.Skip((page - 1) * items_per_page)
										.Limit(items_per_page)
										.ToListAsync();

            return notifications;
        }

        public async Task<List<NotificationMongo>> GetSystemNotification(string typeName, int page, int items_per_page)
        {
			var typeNoti = await _db.TypeNotis.FirstOrDefaultAsync(t_n => t_n.Name == typeName);
			var typeId = typeNoti.Id;

			var filterNotis = Builders<NotificationMongo>.Filter.Eq(n => n.TypeId, typeId);

            var notifications = await _notificationsMongo
                                        .Find(filterNotis)
                                        .SortByDescending(n => n.CreateDate)
                                        .Skip((page - 1) * items_per_page)
                                        .Limit(items_per_page)
                                        .ToListAsync();
            return notifications;
        }

        //Get 1 Notification By Id
        public async Task<NotificationMongo> GetSingleNotification(string notificationId)
        {
            return await _notificationsMongo.Find(notification => notification.Id == notificationId).FirstOrDefaultAsync();
        }

        public async Task InsertManyAsync(List<Notification_UserMongo> notification_user)
        {
            await _notification_userMongo.InsertManyAsync(notification_user);
        }

        //Mark as Read Notification
        public async Task MarkAsRead(string userId, string NotificationId)
        {
			var notificationUser = Builders<Notification_UserMongo>.Filter.Eq(n => n.NotificationId, NotificationId) & 
                                    Builders<Notification_UserMongo>.Filter.Eq(n => n.UserId, userId);

            var update = Builders<Notification_UserMongo>.Update.Set(n => n.isRead, true);

            await _notification_userMongo.UpdateOneAsync(notificationUser, update);
        }

        public async Task ClearNotificationsAsync()
        {
            await _notificationsMongo.DeleteManyAsync(Builders<NotificationMongo>.Filter.Empty);
        }

        public async Task ClearNotificationsUserAsync()
        {
            await _notification_userMongo.DeleteManyAsync(Builders<Notification_UserMongo>.Filter.Empty);
        }

	}
}
