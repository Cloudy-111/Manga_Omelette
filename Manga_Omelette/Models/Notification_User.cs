using Manga_Omelette.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Omelette.Models
{
    public class Notification_User
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Notification")]
        public string NotificationId { get; set; }
        public Notification Notification { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public bool isRead { get; set; }
    }
}
