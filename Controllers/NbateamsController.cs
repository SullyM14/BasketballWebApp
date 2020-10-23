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

        // GET: Nbateams/Details/5s
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nbateams = await _crud.RetrieveNbaTeams()
                .FirstOrDefaultAsync(m => m.NbateamId == id);
            if (nbateams == null)
            {
                return NotFound();
            }

            return View(nbateams);
        }

        //GET: Nbateams/Players/5s
        //public async Task<IActionResult> Players(int? id)
        //{
        //    var players = _context.Players.Where(p => p.TeamId == id).OrderByDescending(p=>p.Price);
        //    return View(await players.ToListAsync());
        //}

        // GET: Nbateams/PlayerDetails/5s
        //public async Task<IActionResult> PlayerDetails(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var player = await _context.Players
        //        .FirstOrDefaultAsync(p=>p.PlayerId == id);
        //    if (player == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(player);
        //}

        //private bool NbateamsExists(int id)
        //{
        //    return _context.Nbateams.Any(e => e.NbateamId == id);
        //}
    }
}
