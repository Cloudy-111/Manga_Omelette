using System.ComponentModel.DataAnnotations;

namespace Manga_Omelette.Models
{
    public class TypeNotis
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
