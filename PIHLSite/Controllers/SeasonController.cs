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
    public class SeasonController : Controller
    {
        private readonly PIHLDBContext _context;

        public SeasonController(PIHLDBContext context)
        {
            _context = context;
        }

        // GET: Season
        public async Task<IActionResult> Index()
        {
            return View(await _context.Seasons.ToListAsync());
        }

        // GET: Season/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // GET: Season/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Season/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeasonId,StartYear,EndYear")] Season season)
        {
            if (ModelState.IsValid)
            {
                _context.Add(season);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(season);
        }

        // GET: Season/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }
            return View(season);
        }

        // POST: Season/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeasonId,StartYear,EndYear")] Season season)
        {
            if (id != season.SeasonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(season);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonExists(season.SeasonId))
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
            return View(season);
        }

        // GET: Season/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Season/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.SeasonId == id);
        }
    }
}
