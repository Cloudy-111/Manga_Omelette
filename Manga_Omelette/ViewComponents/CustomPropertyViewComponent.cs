using Manga_Omelette.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Manga_Omelette.ViewComponents
{
    public class CustomPropertyViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        public CustomPropertyViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        //display Property NameDisplay of User by UserId
        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Content("User not found");
            }
            var NameDisplay = user.NameDisplay ?? "GUEST";
            return View("Default", NameDisplay);
        }
    }
}
