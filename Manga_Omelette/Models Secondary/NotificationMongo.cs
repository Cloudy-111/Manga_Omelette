using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Manga_Omelette.Models_Secondary
{
	public class NotificationMongo
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public string Id { get; set; }

		[BsonElement("title")]
		[Required]
		public string Title { get; set; }

		[BsonElement("content")]
		[Required]
		public string Content { get; set; }

		[BsonElement("createDate")]
		public DateTime CreateDate { get; set; } = DateTime.UtcNow;

		[BsonElement("senderId")]
		[Required]
		public string SenderId { get; set; }

		[BsonElement("receiverId")]
		public string ReceiverId { get; set; }

		[BsonElement("typeId")]
		public string TypeId { get; set; }

		[BsonElement("storyId")]
		public int StoryId { get; set; } = 0;
	}
}
