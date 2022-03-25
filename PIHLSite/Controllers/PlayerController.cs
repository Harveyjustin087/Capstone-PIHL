using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIHLSite.Models;

namespace PIHLSite.Controllers
{
    public class PlayerController : Controller
    {
        private readonly PIHLDBContext _context;

        public PlayerController(PIHLDBContext context)
        {
            _context = context;
        }

        // GET: Player
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string adminUser = User.Identity.Name.ToString();
            if (adminUser == "christopher.thoms@colliers.com" || adminUser == "Christopher.Thoms@colliers.com")
            {
                var pIHLDBContext = _context.Players.Include(p => p.Team).Where(p => p.FirstName != "No Player");
                return View(await pIHLDBContext.ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Player/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Player/Create
        [Authorize]
        public IActionResult Create()
        {
            string adminUser = User.Identity.Name.ToString();
            if (adminUser == "christopher.thoms@colliers.com" || adminUser == "Christopher.Thoms@colliers.com")
            {
                ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name");
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Player/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PlayerId,FirstName,LastName,Age,ScoreTotal,AssistTotal,PointTotal,Pimtotal,TeamId,JerseyNumber")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", player.TeamId);
            return View(player);
        }

        // GET: Player/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            string adminUser = User.Identity.Name.ToString();
            if (adminUser == "christopher.thoms@colliers.com" || adminUser == "Christopher.Thoms@colliers.com")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var player = await _context.Players.FindAsync(id);
                if (player == null)
                {
                    return NotFound();
                }
                ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", player.TeamId);
                return View(player);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Player/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PlayerId,FirstName,LastName,Age,ScoreTotal,AssistTotal,PointTotal,Pimtotal,TeamId,JerseyNumber")] Player player)
        {
            if (id != player.PlayerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.PlayerId))
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
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Company", player.TeamId);
            return View(player);
        }

        // GET: Player/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            string adminUser = User.Identity.Name.ToString();
            if (adminUser == "christopher.thoms@colliers.com" || adminUser == "Christopher.Thoms@colliers.com")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var player = await _context.Players
                    .Include(p => p.Team)
                    .FirstOrDefaultAsync(m => m.PlayerId == id);
                if (player == null)
                {
                    return NotFound();
                }

                return View(player);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}