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
using Microsoft.AspNetCore.Authorization;
using Manga_Omelette.Services;

namespace Manga_Omelette.Controllers
{
    public class TypeNotisController : Controller
    {
        private readonly Manga_OmeletteDBContext _db;
        private readonly NotificationService _notificationService;

        public TypeNotisController(Manga_OmeletteDBContext db, NotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }

        // GET: TypeNotis
        public async Task<IActionResult> Index()
        {
            return View(await _db.TypeNotis.ToListAsync());
        }

        // GET: TypeNotis/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeNotis = await _db.TypeNotis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeNotis == null)
            {
                return NotFound();
            }

            return View(typeNotis);
        }

        //==============================================Page Create Type Notification==============================================
        [Authorize(Roles = "Super ADMIN")]
        public IActionResult CreateTypeNoti()
        {
            return View();
        }

        [Authorize(Roles = "Super ADMIN")]
        //==============================================Post Create Type Notification==============================================
        [HttpPost]
        public IActionResult CreateTypeNoti(CreateTypeNotiViewModel model)
        {
            if (ModelState.IsValid)
            {
                TypeNotis type = new TypeNotis()
                {
                    Name = model?.TypeName
                };
                _db.Add(type);
                _db.SaveChanges();
                return RedirectToAction("ManageNotification", "Administration");
            }
            return View(model);
        }

        // GET: TypeNotis/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeNotis = await _db.TypeNotis.FindAsync(id);
            if (typeNotis == null)
            {
                return NotFound();
            }
            return View(typeNotis);
        }

        // POST: TypeNotis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] TypeNotis typeNotis)
        {
            if (id != typeNotis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(typeNotis);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeNotisExists(typeNotis.Id))
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
            return View(typeNotis);
        }

        // GET: TypeNotis/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeNotis = await _db.TypeNotis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeNotis == null)
            {
                return NotFound();
            }

            return View(typeNotis);
        }

        // POST: TypeNotis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var typeNotis = await _db.TypeNotis.FindAsync(id);
            if (typeNotis != null)
            {
                _db.TypeNotis.Remove(typeNotis);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeNotisExists(string id)
        {
            return _db.TypeNotis.Any(e => e.Id == id);
        }

        public IActionResult NotificationInType(string typeId, int page = 1)
        {
            var type = _db.TypeNotis.FirstOrDefault(t => t.Id == typeId);
            if(type == null)
            {
                return NotFound(typeId);
            }
            int items_per_page = 10;
            var result = _notificationService.GetNotificationForEachPage(page, items_per_page, typeId);

            int totalNotification = _db.Notification.Where(n => n.TypeId == typeId).Count();
            int totalPages = (int)Math.Ceiling((double)totalNotification / items_per_page);
            
            ViewBag.TypeId = typeId;
            ViewBag.TypeName = type.Name;
            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;
            return View(result);
        }
    }
}
