using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Manga_Omelette.Data;
using Manga_Omelette.Models;

namespace Manga_Omelette
{
    public class Notification_UserController : Controller
    {
        private readonly Manga_OmeletteDBContext _context;

        public Notification_UserController(Manga_OmeletteDBContext context)
        {
            _context = context;
        }

        // GET: Notification_User
        public async Task<IActionResult> Index()
        {
            var manga_OmeletteDBContext = _context.Notification_User.Include(n => n.Notification).Include(n => n.User);
            return View(await manga_OmeletteDBContext.ToListAsync());
        }

        // GET: Notification_User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification_User = await _context.Notification_User
                .Include(n => n.Notification)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification_User == null)
            {
                return NotFound();
            }

            return View(notification_User);
        }

        // GET: Notification_User/Create
        public IActionResult Create()
        {
            ViewData["NotificationId"] = new SelectList(_context.Notification, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Notification_User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NotificationId,UserId,isRead")] Notification_User notification_User)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification_User);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationId"] = new SelectList(_context.Notification, "Id", "Id", notification_User.NotificationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", notification_User.UserId);
            return View(notification_User);
        }

        // GET: Notification_User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification_User = await _context.Notification_User.FindAsync(id);
            if (notification_User == null)
            {
                return NotFound();
            }
            ViewData["NotificationId"] = new SelectList(_context.Notification, "Id", "Id", notification_User.NotificationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", notification_User.UserId);
            return View(notification_User);
        }

        // POST: Notification_User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NotificationId,UserId,isRead")] Notification_User notification_User)
        {
            if (id != notification_User.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification_User);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Notification_UserExists(notification_User.Id))
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
            ViewData["NotificationId"] = new SelectList(_context.Notification, "Id", "Id", notification_User.NotificationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", notification_User.UserId);
            return View(notification_User);
        }

        // GET: Notification_User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification_User = await _context.Notification_User
                .Include(n => n.Notification)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification_User == null)
            {
                return NotFound();
            }

            return View(notification_User);
        }

        // POST: Notification_User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification_User = await _context.Notification_User.FindAsync(id);
            if (notification_User != null)
            {
                _context.Notification_User.Remove(notification_User);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Notification_UserExists(int id)
        {
            return _context.Notification_User.Any(e => e.Id == id);
        }
    }
}
