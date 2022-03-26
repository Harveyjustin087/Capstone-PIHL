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
    public class PenaltyRecordController : Controller
    {
        private readonly PIHLDBContext _context;

        public PenaltyRecordController(PIHLDBContext context)
        {
            _context = context;
        }

        // GET: PenaltyRecord
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            var pIHLDBContext = _context.PenaltyRecords.Include(p => p.Game).Include(p => p.Penalty).Include(p => p.Player).Where(p => p.GameId == id);
            return View(await pIHLDBContext.ToListAsync());
        }

        // GET: PenaltyRecord/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penaltyRecord = await _context.PenaltyRecords
                .Include(p => p.Game)
                .Include(p => p.Penalty)
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.PenaltyRecordId == id);
            if (penaltyRecord == null)
            {
                return NotFound();
            }

            return View(penaltyRecord);
        }

        // GET: PenaltyRecord/Create
        [Authorize]
        public IActionResult Create(int id)
        {
            var gameRecord = _context.Set<Game>().FirstOrDefault(o => o.GameId == id);
            ViewData["GameId"] = gameRecord.GameId;
            ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription");
            ViewData["PlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
            return View();
        }

        // POST: PenaltyRecord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PenaltyRecordId,GameId,PlayerId,PenaltyId,Pim")] PenaltyRecord penaltyRecord)
        {
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == penaltyRecord.GameId);
            var playerRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == penaltyRecord.PlayerId);
            if (gameRecord == null)
            {
                return NotFound();
            }
            TimeSpan duration = new TimeSpan(60,00,0);
            if (penaltyRecord.Pim > duration)
            {
                ViewData["GameId"] = gameRecord.GameId;
                ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription");
                ViewData["PlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                return View();
            }
            if (playerRecord != null)
            {
                playerRecord.Pimtotal += penaltyRecord.Pim;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(penaltyRecord);
                    await _context.SaveChangesAsync();

                    _context.Add(playerRecord);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Penalty Information Created";
                    return Redirect(Url.Action("Index", "Scorekeeper"));
                }
                TempData["Message"] = "Penalty Information Not Created";
                return Redirect(Url.Action("Index", "Scorekeeper"));
            }
            ViewData["GameId"] = gameRecord.GameId;
            ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription");
            ViewData["PlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
            return View(penaltyRecord);
        }

        // GET: PenaltyRecord/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var penaltyRecord = await _context.PenaltyRecords.FindAsync(id);
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == penaltyRecord.GameId);
            if (penaltyRecord == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = gameRecord.GameId;
            ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription");
            ViewData["PlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
            return View(penaltyRecord);
        }

        // POST: PenaltyRecord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PenaltyRecordId,GameId,PlayerId,PenaltyId,Pim")] PenaltyRecord penaltyRecord)
        {
            if (id != penaltyRecord.PenaltyRecordId)
            {
                return NotFound();
            }
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == penaltyRecord.GameId);
            var playerRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == penaltyRecord.PlayerId);
            if (gameRecord == null)
            {
                return NotFound();
            }
            TimeSpan duration = new TimeSpan(60, 00, 0);
            if (penaltyRecord.Pim > duration)
            {
                ViewData["GameId"] = gameRecord.GameId;
                ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription");
                ViewData["PlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                return View();
            }
            if (playerRecord != null)
            {
                playerRecord.Pimtotal += penaltyRecord.Pim;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(penaltyRecord);
                    await _context.SaveChangesAsync();

                    _context.Update(playerRecord);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PenaltyRecordExists(penaltyRecord.PenaltyRecordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Penalty Information Updated";
                return RedirectToAction("Index", "Scorekeeper");
            }
            TempData["Message"] = "Penalty Information Not Updated";
            return RedirectToAction("Index", "Scorekeeper");
        }

        // GET: PenaltyRecord/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var penaltyRecord = await _context.PenaltyRecords
                .Include(p => p.Game)
                .Include(p => p.Penalty)
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.PenaltyRecordId == id);
            if (penaltyRecord == null)
            {
                return NotFound();
            }

            return View(penaltyRecord);
        }

        // POST: PenaltyRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var penaltyRecord = await _context.PenaltyRecords.FindAsync(id);
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == penaltyRecord.GameId);
            var playerRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == penaltyRecord.PlayerId);
            if (gameRecord == null)
            {
                return NotFound();
            }
            if (playerRecord != null)
            {
                playerRecord.Pimtotal -= penaltyRecord.Pim;
            }
            try
            {
                _context.PenaltyRecords.Remove(penaltyRecord);
                await _context.SaveChangesAsync();

                _context.Players.Add(playerRecord);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["Message"] = "Penalty Information Deleted";
                return Redirect(Url.Action("Index", "Scorekeeper"));
            }
            TempData["Message"] = "Penalty Information Deleted";
            return Redirect(Url.Action("Index", "Scorekeeper"));
        }

        private bool PenaltyRecordExists(int id)
        {
            return _context.PenaltyRecords.Any(e => e.PenaltyRecordId == id);
        }
    }
}