using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BasketballWebApp.Models;
using BasketballWebApp.Service;

namespace BasketballWebApp.Controllers
{
    public class UserTeamPlayersController : Controller
    {
        private readonly ICRUD _crud;
        public UserTeamPlayersController(ICRUD crud)
        {
            _crud = crud;
        }

        // GET: UserTeamPlayers
        public async Task<IActionResult> Index(int? id)
        {
            var userTeamPlayers = _crud.RetrieveUserTeamsPlayers(id);

            return View(await userTeamPlayers.ToListAsync());
        }

        //// GET: UserTeamPlayers/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userTeamPlayers = await _context.UserTeamPlayers
        //        .Include(u => u.Player)
        //        .Include(u => u.UserTeam)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userTeamPlayers == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userTeamPlayers);
        //}

        // GET: UserTeamPlayers/Create/5
        public IActionResult Create(int? id)
        {
            ViewData["PlayerId"] = new SelectList(_crud.RetrievePlayers(), "PlayerId", "FirstName", "LastName");
            ViewData["UserTeamId"] = new SelectList(_crud.RetrieveUserTeam(id), "UserTeamId", "TeamName");
            return View();
        }

        // POST: UserTeamPlayers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserTeamId,PlayerId,Id")] UserTeamPlayers userTeamPlayers)
        {
            if (ModelState.IsValid)
            {
                await _crud.AddPlayer(userTeamPlayers);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerId"] = new SelectList(_crud.RetrievePlayers(), "PlayerId", "FirstName", "LastName");
            ViewData["UserTeamId"] = new SelectList(_crud.RetrieveUserTeam(userTeamPlayers.UserTeamId), "UserTeamId", "TeamName");
            return View(userTeamPlayers);
        }

        //// GET: UserTeamPlayers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userTeamPlayers = await _context.UserTeamPlayers.FindAsync(id);
        //    if (userTeamPlayers == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "FirstName", userTeamPlayers.PlayerId);
        //    ViewData["UserTeamId"] = new SelectList(_context.UserTeams, "UserTeamId", "TeamName", userTeamPlayers.UserTeamId);
        //    return View(userTeamPlayers);
        //}

        //// POST: UserTeamPlayers/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserTeamId,PlayerId,Id")] UserTeamPlayers userTeamPlayers)
        //{
        //    if (id != userTeamPlayers.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(userTeamPlayers);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserTeamPlayersExists(userTeamPlayers.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "FirstName", userTeamPlayers.PlayerId);
        //    ViewData["UserTeamId"] = new SelectList(_context.UserTeams, "UserTeamId", "TeamName", userTeamPlayers.UserTeamId);
        //    return View(userTeamPlayers);
        //}

        //// GET: UserTeamPlayers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userTeamPlayers = await _context.UserTeamPlayers
        //        .Include(u => u.Player)
        //        .Include(u => u.UserTeam)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userTeamPlayers == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userTeamPlayers);
        //}

        //// POST: UserTeamPlayers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var userTeamPlayers = await _context.UserTeamPlayers.FindAsync(id);
        //    _context.UserTeamPlayers.Remove(userTeamPlayers);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserTeamPlayersExists(int id)
        //{
        //    return _context.UserTeamPlayers.Any(e => e.Id == id);
        //}
    }
}
