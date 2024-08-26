using Manga_Omelette.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class ListNotificationsViewModel
    {
        public IQueryable<Notification> SystemNotifications { get; set; }
        public List<Notification> AdminNotificatons { get; set; }
        public string tabActive { get; set; }
    }
}
