using Microsoft.AspNetCore.Identity;

namespace Manga_Omelette.Models_Secondary
{
    public class ManageUserViewModel
    {
        public List<IdentityRole> roles {  get; set; }
    }
}
