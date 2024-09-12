using Manga_Omelette.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class ListNotificationsViewModel
    {
        public List<NotificationMongo> SystemNotifications { get; set; }
        public List<NotificationMongo> AdminNotificatons { get; set; }
        public string tabActive { get; set; }
    }
}
