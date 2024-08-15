using System.ComponentModel.DataAnnotations;

namespace Manga_Omelette.Models_Secondary
{
    public class CreateTypeNotiViewModel
    {
        [Required]
        [Display(Name = "Type")]
        public string TypeName { get; set; }
    }
}
