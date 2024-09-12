using MongoDB.Bson.Serialization.Attributes;

namespace Manga_Omelette.Models_Secondary
{
	public class Notification_UserMongo
	{
		[BsonId]
		public string Id { get; set; }
		[BsonElement("NotiId")]
		public string NotificationId { get; set; }

		[BsonElement("UserId")]
		public string UserId { get; set; }

		[BsonElement("MarkAsRead")]
		public bool isRead { get; set; } = false; //Not read on default
	}
}
