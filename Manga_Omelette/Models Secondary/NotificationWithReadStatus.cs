using MongoDB.Bson.Serialization.Attributes;

namespace Manga_Omelette.Models_Secondary
{
    public class NotificationWithReadStatus
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string TypeId { get; set; }
        public int StoryId { get; set; }
        public bool isRead { get; set; }
    }
}
