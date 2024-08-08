namespace Manga_Omelette.Models_Secondary
{
	public class AuthorWithRoles
	{
		public int AuthorId { get; set; }
		public string Name { get; set; }
		public bool isAuthor { get; set; }
		public bool isArtist { get; set; }
	}
}
