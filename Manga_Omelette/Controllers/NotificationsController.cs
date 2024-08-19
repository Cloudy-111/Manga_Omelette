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

namespace Manga_Omelette.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly Manga_OmeletteDBContext _db;

        public NotificationsController(Manga_OmeletteDBContext db)
        {
            _db = db;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            return View(await _db.Notification.ToListAsync());
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
            var notification = _db.Notification.Include(n => n.TypeNotis).FirstOrDefault(n => n.Id == notiId);
            if (notification == null || notification.TypeNotis == null)
            {
                return NotFound();
            }
            return Json(new { type = notification.TypeNotis.Name });
        }
    }
}
