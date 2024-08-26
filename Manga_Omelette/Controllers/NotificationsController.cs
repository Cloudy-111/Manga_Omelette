using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Manga_Omelette.Data;
using Manga_Omelette.Models;
using Manga_Omelette.Models_Secondary;
using Manga_Omelette.Services;
using Microsoft.AspNetCore.Identity;
using Manga_Omelette.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Manga_Omelette.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly NotificationService _notificationService;

        private readonly UserManager<User> _userManager;

        public NotificationsController(Manga_OmeletteDBContext db, NotificationService notificationService, UserManager<User> userManager)
        {
            _db = db;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        // GET: Notifications
        public async Task<IActionResult> Index(string tab, int pageSystem = 1, int pageAdmin = 1)
        {
            var items_per_page = 10;
            var SystemNotification = _notificationService.GetSystemNotification("System", pageSystem, items_per_page);

            int totalSystemNotifications = _db.Notification.Where(n => n.TypeNotis.Name.ToLower() == "system").Count();
            int totalPageSystemNotification = (int) Math.Ceiling((double)totalSystemNotifications / items_per_page);

            var userId = _userManager.GetUserId(User);
            var AdminNotification = new List<Notification>();
            int totalPageAdminNotification = 0;
            if (userId != null)
            {
                AdminNotification = _notificationService.GetAdminNotification(userId, pageAdmin, items_per_page).ToList();

                int totalAdminNotifications = _db.Notification.Where(n => n.Notification_User.Any(n_u => n_u.UserId == userId)).Count();
                totalPageAdminNotification = (int)Math.Ceiling((double)totalAdminNotifications / items_per_page);
            }

            ViewBag.TotalPageSystemNotification = totalPageSystemNotification;
            ViewBag.TotalPageAdminNotification = totalPageAdminNotification;

            var ListNotificationViewModel = new ListNotificationsViewModel
            {
                SystemNotifications = SystemNotification,
                AdminNotificatons = AdminNotification,
                tabActive = tab ?? "system"
            };
            return View(ListNotificationViewModel);
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _db.Notification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            var CreateNotificationViewModel = new CreateNotificationViewModel
            {
                newNotification = new Notification(),
                types = _db.TypeNotis.ToList(),
            };
            return View(CreateNotificationViewModel);
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateNotificationViewModel model)
        {
            if (model.newNotification != null)
            {
                model.newNotification.Id = Guid.NewGuid().ToString();
                model.newNotification.CreateDate = DateTime.Now;
                _db.Add(model.newNotification);
                await _db.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    notification = model.newNotification,
                    redirectUrl = Url.Action("ManageNotification", "Administration")
                });
            }

            return View(model);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _db.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Content,CreateDate,SenderId,ReceiverId")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(notification);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _db.Notification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var notification = await _db.Notification.FindAsync(id);
            if (notification != null)
            {
                _db.Notification.Remove(notification);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(string id)
        {
            return _db.Notification.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult GetNotificationType(string notificationId)
        {
            var notification = _db.Notification.Include(n => n.TypeNotis).FirstOrDefault(n => n.Id == notificationId);
            if (notification == null)
            {
                return NotFound();
            }
            return Json(new { type = notification.TypeNotis.Name });
        }
    }
}
