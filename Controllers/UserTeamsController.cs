﻿using System;
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
            ViewData["UserName"] = _crud.RetrieveUserName(1);
            return View(await fantasyTeam.ToListAsync());
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

        // POST: UserTeams/Delete/16011
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            _crud.RemoveUserTeam(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Userteams/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: Userteams/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to, for 
         //more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserTeamId, UserId, TeamName")] UserTeams userTeam)
        {
            if (ModelState.IsValid)
            {
                await _crud.CreateNewTeam(userTeam);
                return RedirectToAction(nameof(Index));
            }
            return View(userTeam);
        }
    }
}
