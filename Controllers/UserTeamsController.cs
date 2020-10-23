using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BasketballWebApp.Models;

namespace BasketballWebApp.Controllers
{
    public class UserTeamsController : Controller
    {
        private readonly BasketballProjectContext _context;

        public UserTeamsController(BasketballProjectContext context)
        {
            _context = context;
        }

        // GET: UserTeams
        public async Task<IActionResult> Index()
        {
            var basketballProjectContext = _context.UserTeams.Include(u => u.User);
            return View(await basketballProjectContext.ToListAsync());
        }

        // GET: UserTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTeams = await _context.UserTeams
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserTeamId == id);
            if (userTeams == null)
            {
                return NotFound();
            }

            return View(userTeams);
        }

        // GET: UserTeams/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName");
            return View();
        }

        // POST: UserTeams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserTeamId,UserId,Budget,TeamName")] UserTeams userTeams)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTeams);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", userTeams.UserId);
            return View(userTeams);
        }

        // GET: UserTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTeams = await _context.UserTeams.FindAsync(id);
            if (userTeams == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", userTeams.UserId);
            return View(userTeams);
        }

        // POST: UserTeams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserTeamId,UserId,Budget,TeamName")] UserTeams userTeams)
        {
            if (id != userTeams.UserTeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTeams);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTeamsExists(userTeams.UserTeamId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", userTeams.UserId);
            return View(userTeams);
        }

        // GET: UserTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTeams = await _context.UserTeams
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserTeamId == id);
            if (userTeams == null)
            {
                return NotFound();
            }

            return View(userTeams);
        }

        // POST: UserTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTeams = await _context.UserTeams.FindAsync(id);
            _context.UserTeams.Remove(userTeams);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTeamsExists(int id)
        {
            return _context.UserTeams.Any(e => e.UserTeamId == id);
        }
    }
}
