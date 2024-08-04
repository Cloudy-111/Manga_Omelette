using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{//This class to take NameDisplay(belong to Model User) to comment
	public class Comment_User
	{
		public int Id { get; set; }
		public string comment_content { get; set; }
		public string CreatedDate { get; set; }
		public string userId { get; set; }
		public string userNameDisplay { get; set; }
		public int? reply_amount { get; set; }
	}
}
