using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Omelette.Models
{
    public class Notification
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        [Required]
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        [ForeignKey("TypeNotis")]
        public string TypeId { get; set; }
        public TypeNotis TypeNotis { get; set; }
    }
}
