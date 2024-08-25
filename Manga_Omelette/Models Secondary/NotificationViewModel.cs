namespace Manga_Omelette.Models_Secondary
{
    public class NotificationViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string TypeId { get; set; }
        public string SenderId { get; set; }
    }
}
