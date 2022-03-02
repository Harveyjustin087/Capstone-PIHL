using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index(int id)
        {
            var pIHLDBContext = _context.PenaltyRecords.Include(p => p.Game).Include(p => p.Penalty).Include(p => p.Player).Where(p => p.GameId == id);
            return View(await pIHLDBContext.ToListAsync());
        }

        // GET: PenaltyRecord/Details/5
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
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription");
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "LastName");
            return View();
        }

        // POST: PenaltyRecord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PenaltyRecordId,GameId,PlayerId,PenaltyId,Pim")] PenaltyRecord penaltyRecord)
        {
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == penaltyRecord.GameId);
            var playerRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == penaltyRecord.PlayerId);
            if (gameRecord == null)
            {
                return NotFound();
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
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", penaltyRecord.GameId);
            ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription", penaltyRecord.PenaltyId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "LastName", penaltyRecord.PlayerId);
            return View(penaltyRecord);
        }

        // GET: PenaltyRecord/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var penaltyRecord = await _context.PenaltyRecords.FindAsync(id);
            if (penaltyRecord == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", penaltyRecord.GameId);
            ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyDescription", penaltyRecord.PenaltyId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "LastName", penaltyRecord.PlayerId);
            return View(penaltyRecord);
        }

        // POST: PenaltyRecord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PenaltyRecordId,GameId,PlayerId,PenaltyId,Pim")] PenaltyRecord penaltyRecord)
        {
            if (id != penaltyRecord.PenaltyRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(penaltyRecord);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", penaltyRecord.GameId);
            ViewData["PenaltyId"] = new SelectList(_context.Penalties, "PenaltyId", "PenaltyCode", penaltyRecord.PenaltyId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "FirstName", penaltyRecord.PlayerId);
            return View(penaltyRecord);
        }

        // GET: PenaltyRecord/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var penaltyRecord = await _context.PenaltyRecords.FindAsync(id);
            _context.PenaltyRecords.Remove(penaltyRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PenaltyRecordExists(int id)
        {
            return _context.PenaltyRecords.Any(e => e.PenaltyRecordId == id);
        }
    }
}
