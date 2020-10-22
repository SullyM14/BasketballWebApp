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
    public class NbateamsController : Controller
    {
        private readonly BasketballProjectContext _context;

        public NbateamsController(BasketballProjectContext context)
        {
            _context = context;
        }

        // GET: Nbateams
        public async Task<IActionResult> Index()
        {
            var teams = _context.Nbateams.OrderByDescending(t=>t.Wins);
            return View(await teams.ToListAsync());
        }

        // GET: Nbateams/Details/5s
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nbateams = await _context.Nbateams
                .FirstOrDefaultAsync(m => m.NbateamId == id);
            if (nbateams == null)
            {
                return NotFound();
            }

            return View(nbateams);
        }

        //GET: Nbateams/Players/5s
        public async Task<IActionResult> Players(int? id)
        {
            var players = _context.Players.Where(p => p.TeamId == id).OrderByDescending(p=>p.Price);
            return View(await players.ToListAsync());
        }

        // GET: Nbateams/PlayerDetails/5s
        public async Task<IActionResult> PlayerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(p=>p.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        private bool NbateamsExists(int id)
        {
            return _context.Nbateams.Any(e => e.NbateamId == id);
        }
    }
}
