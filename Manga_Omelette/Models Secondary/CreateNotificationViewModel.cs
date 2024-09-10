using Manga_Omelette.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class CreateNotificationViewModel
    {
        //public Notification newNotification { get; set; }
        public List<TypeNotis> types { get; set; }
        public NotificationMongo newNotification { get; set; }
    }
}
