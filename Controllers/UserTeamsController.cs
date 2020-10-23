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
    public class UserTeamsController : Controller
    {
        private readonly ICRUD _crud;

        public UserTeamsController(ICRUD crud)
        {
            _crud = crud;
        }

        // GET: UserTeams
        public async Task<IActionResult> Index()
        {
            var fantasyTeam = _crud.AllUserTeams();
            return View(await fantasyTeam.ToListAsync());
        }


        //GET: UserTeams/FantasyPlayers/5
        public async Task<IActionResult> FantasyPlayers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var FantasyPlayers = _crud.RetrieveUserTeamsPlayers(id);
            if (FantasyPlayers == null)
            {
                return NotFound();
            }
            return View(await FantasyPlayers.ToListAsync());
        }


        // GET: Userteams/PlayerDetails/5s
        public async Task<IActionResult> PlayerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _crud.RetrievePlayerDetails(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }


        // GET: Userteams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTeam = await _crud.RetrieveUserTeamsDetails(id);
            if (userTeam == null)
            {
                return NotFound();
            }

            return View(userTeam);
        }

        // POST: Userteams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _crud.RemoveUserTeam(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
