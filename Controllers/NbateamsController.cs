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
    public class NbateamsController : Controller
    {
        private readonly ICRUD _crud;

        public NbateamsController(ICRUD crud)
        {
            _crud = crud;
        }

        // GET: Nbateams
        public async Task<IActionResult> Index()
        {
            var teams = _crud.RetrieveNbaTeams().ToListAsync();
            return View(await teams);
        }

        //GET: Nbateams/Players/5s
        public async Task<IActionResult> Players(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var players = _crud.RetrieveTeamPlayers(id);
            if (players == null)
            {
                return NotFound();
            }
            return View(await players.ToListAsync());
        }

       // GET: Nbateams/PlayerDetails/5s
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

        //private bool NbateamsExists(int id)
        //{
        //    return _context.Nbateams.Any(e => e.NbateamId == id);
        //}
    }
}
