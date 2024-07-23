using System.ComponentModel.DataAnnotations;

namespace Manga_Omelette.Models_Secondary
{
	public class CreateRoleViewModel
	{
		[Required]
		[Display(Name = "Role")]
		public string RoleName { get; set; }
	}
}
