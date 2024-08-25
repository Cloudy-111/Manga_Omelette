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
    }
}
