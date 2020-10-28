using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BasketballWebApp.Models;
using BasketballWebApp.Service;
using BasketballBusinessLayer;
using System.Security.Principal;

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
            ViewData["UserTeamIdInt"] = _crud.RetrieveUserTeam(id).FirstOrDefault().UserTeamId;
            ViewData["Budget"] = $"£{_crud.RetrieveUserTeam(id).FirstOrDefault().Budget}0";
            ViewData["UserTeamName"] = _crud.RetrieveUserTeam(id).FirstOrDefault().TeamName;
            return View(await userTeamPlayers.ToListAsync());
        }

        // GET: UserTeamPlayers/Create/5
        public IActionResult Create(int? id)
        {
            ViewData["Budget"] = $"£{_crud.RetrieveUserTeam(id).FirstOrDefault().Budget}0";
            ViewData["PlayerId"] = new SelectList(_crud.RetrievePlayers(), "PlayerId", "FirstName", "LastName");
            ViewBag.PlayerId = _crud.RetrievePlayers().Select(p => new SelectListItem
            {
                Text = p.FirstName + " " + p.LastName + ": £" + p.Price + "0",
                Value = p.PlayerId.ToString()
            });
            ViewData["UserTeamName"] = _crud.RetrieveUserTeam(id).FirstOrDefault().TeamName;
            ViewData["UserTeamId"] = new SelectList(_crud.RetrieveUserTeam(id), "UserTeamId", "TeamName");
            ViewData["UserTeamIdInt"] = _crud.RetrieveUserTeam(id).FirstOrDefault().UserTeamId;
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
                try
                {
                    await _crud.AddPlayer(userTeamPlayers);
                    return RedirectToAction("Index", new { id = userTeamPlayers.UserTeamId });
                }
                catch (OutOfBudgetException)
                {
                    ViewData["Error"] = "Out of Budget";
                    return RedirectToAction("Error","Home");
                }
            }
            ViewData["Budget"] = $"£{_crud.RetrieveUserTeam(userTeamPlayers.UserTeamId).FirstOrDefault().Budget}0";
            ViewData["PlayerId"] = new SelectList(_crud.RetrievePlayers(), "PlayerId", "FirstName", "LastName");
            ViewData["UserTeamName"] = _crud.RetrieveUserTeam(userTeamPlayers.UserTeamId).FirstOrDefault().TeamName;
            ViewBag.PlayerId = _crud.RetrievePlayers().Select(p => new SelectListItem
            {
                Text = p.FirstName + " " + p.LastName + ": £" + p.Price + "0",
                Value = p.PlayerId.ToString()
            });
            ViewData["UserTeamId"] = new SelectList(_crud.RetrieveUserTeam(userTeamPlayers.UserTeamId), "UserTeamId", "TeamName");

            return View(userTeamPlayers);
        }

        // GET: UserTeamPlayers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTeamPlayers = await _crud.RetrieveUserPlayer(id);
            if (userTeamPlayers == null)
            {
                return NotFound();
            }
            ViewData["UserTeamName"] = _crud.RetrieveUserTeam(userTeamPlayers.UserTeamId).FirstOrDefault().TeamName;
            ViewData["PlayerName"] = $"{userTeamPlayers.Player.FirstName} {userTeamPlayers.Player.LastName}";
            ViewData["UserTeamIdInt"] = userTeamPlayers.UserTeamId;
            return View(userTeamPlayers);
        }

        // POST: UserTeamPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, int? userTeamId)
        {
            await _crud.RemoveUserPlayer(id);
            return RedirectToAction("Index", new {id = userTeamId});
           // return RedirectToAction(nameof(Index));
        }

    }
}
