using CloudinaryDotNet.Actions;
using Manga_Omelette.Areas.Identity.Data;
using Manga_Omelette.Data;
using Manga_Omelette.Models_Secondary;
using Manga_Omelette.Services;
using MangaASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Manga_Omelette.Controllers
{
	[Authorize(Roles = "Super ADMIN")]
	public class AdministrationController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly Manga_OmeletteDBContext _db;
		private readonly UserManager<User> _userManager;
		private readonly StoryService _storyService;
		private readonly CloudinaryService _cloudinaryService;

        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public AdministrationController(RoleManager<IdentityRole> roleManager, Manga_OmeletteDBContext db, UserManager<User> userManager, StoryService storyService, CloudinaryService cloudinaryService)
		{
			_roleManager = roleManager;
			_db = db;
			_userManager = userManager;
			_storyService = storyService;
			_cloudinaryService = cloudinaryService;
		}
		public IActionResult DashboardSuperAdmin()
		{
			return View();
		}

		//==============================================Page Manage User==============================================
		public IActionResult ManageUser()
		{
			var ManageUserViewModel = new ManageUserViewModel()
			{
				roles = _roleManager.Roles.ToList(),
			};
			return View(ManageUserViewModel);
		}

		//==============================================Display All User in 1 role==============================================
		public async Task<IActionResult> UsersInRole(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				return NotFound();
			}
			ViewBag.RoleName = role.Name;
			var result = await _userManager.GetUsersInRoleAsync(role.Name);
			return View(result);
		}

        //==============================================Edit Information of 1 User==============================================
        [HttpGet]
		public async Task<IActionResult> EditUser(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			var userRolesViewModels = new List<UserRolesViewModel>();
			foreach(var role in _roleManager.Roles.ToList())
			{
				var userRolesViewModel = new UserRolesViewModel
				{
					roleId = role.Id,
					roleName = role.Name,
				};

				if(await _userManager.IsInRoleAsync(user, role.Name))
				{
					userRolesViewModel.IsSelected = true;
				}
				else
				{
					userRolesViewModel.IsSelected = false;
				}
				userRolesViewModels.Add(userRolesViewModel);
			}
			var model = new EditUserViewModel()
			{
				user = user,
				userRolesViewModels = userRolesViewModels
			};
			return View(model);
		}

        //==============================================Post Information of 1 User==============================================
        [HttpPost]
		public async Task<IActionResult> EditUser(EditUserViewModel model)
		{
			if(model == null)
			{
				return NotFound();
			}

			var user = await _userManager.FindByIdAsync(model.user.Id);
			if(user == null)
			{
                return NotFound($"Unable to load user with ID '{model.user.Id}'.");
            }
			user.Email = model.user.Email;
			user.UserName = model.user.Email;
			user.NameDisplay = model.user.NameDisplay;

            //==============================================Update Attribute of User==============================================
            var updateInformResult = await _userManager.UpdateAsync(user);
			if (!updateInformResult.Succeeded)
			{
                foreach (var error in updateInformResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

			//Get all Roles Exist of User
			var roles = await _userManager.GetRolesAsync(user);
			//Remove all Roles of that user
			var result = await _userManager.RemoveFromRolesAsync(user, roles);

			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Cannot remove user existing roles");
				return View(model);
			}

            //Get all Roles Checked in form
            List<string> RolesChecked = model.userRolesViewModels.Where(c => c.IsSelected).Select(c => c.roleName).ToList();
			if (RolesChecked.Any())
			{
				result = await _userManager.AddToRolesAsync(user, RolesChecked);
				if (!result.Succeeded)
				{
                    ModelState.AddModelError("", "Cannot Add Selected Roles to User");
                    return View(model);
                }
			}
			TempData["UpdateUserSuccess"] = "Update User Success!";
			return RedirectToAction("EditUser", new {userId = user.Id});
		}

		//==============================================Page ManageStory==============================================
		public IActionResult ManageStory(int page = 1)
		{
			var items_per_page = 10;
            IQueryable<Story> storyList = _storyService.GetStoriesForEachPage(page, items_per_page);
            int totalStories = _db.Story.Count();
            int totalPages = (int)Math.Ceiling((double)totalStories / items_per_page);

            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;

            return View(storyList);
		}
		[HttpPost]
		public IActionResult UploadImage(UploadImageModel model)
		{
			if (model.imageFile == null)
			{
				return BadRequest("No file Choosen");
			}
            var ext = Path.GetExtension(model.imageFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return BadRequest("Invalid file type.");
            }
			try
			{
                var uploadResultURL = _cloudinaryService.UploadImage(model.imageFile);
				return RedirectToAction("DashboardSuperAdmin", "Administration");
            }
			catch(Exception ex)
			{
                ModelState.AddModelError(string.Empty, ex.Message);
            }
			return View(model);
        }

		public IActionResult UploadImagetoCloudinary()
		{
			return View();
		}

		//==============================================Page Manage Notification==============================================
		public IActionResult ManageNotification()
		{
			return View();
		}

		//==============================================Page Manage System==============================================
		public IActionResult ManageSystem()
		{
			return View();
		}
		[Authorize(Roles = "Super ADMIN")]

		//==============================================Page Create Role==============================================
		//[HttpGet]
		public IActionResult CreateRole()
		{
			return View();
		}

		//==============================================Post Create Role==============================================
		[HttpPost]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel)
		{
			if(ModelState.IsValid)
			{
				bool roleExists = await _roleManager.RoleExistsAsync(roleModel?.RoleName);
				if (roleExists)
				{
					ModelState.AddModelError("", "Role Already Exist!");
				}
				else
				{
					IdentityRole identityRole = new IdentityRole
					{
						Name = roleModel?.RoleName
					};
					IdentityResult result = await _roleManager.CreateAsync(identityRole);
					if(result.Succeeded)
					{
						return RedirectToAction("DashboardSuperAdmin", "Administration");
					}

					foreach(IdentityError err in result.Errors)
					{
						ModelState.AddModelError("", err.Description);
					}
				}
			}
			return View(roleModel);
		}
	}
}
