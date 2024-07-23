using Manga_Omelette.Areas.Identity.Data;

namespace Manga_Omelette.Models_Secondary
{
    public class EditUserViewModel
    {
        public User user {  get; set; }
        public List<UserRolesViewModel> userRolesViewModels { get; set; }
    }
}
