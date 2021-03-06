using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIHLSite.Models;
using Microsoft.AspNetCore.Identity;
using PIHLSite.Areas.Identity.Data;

namespace PIHLSite.Controllers
{
    public class GoalController : Controller
    {
        private readonly PIHLDBContext _context;

        public GoalController(PIHLDBContext context)
        {
            _context = context;
        }

        // GET: Goal
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            var pIHLDBContext = _context.GoalRecords.Include(g => g.FirstAssistPlayer).Include(g => g.Game).Include(g => g.ScoringPlayer).Include(g => g.SecondAssistPlayer).Where(o => o.GameId == id);
            return View(await pIHLDBContext.ToListAsync());
        }

        // GET: Goal/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goalRecord = await _context.GoalRecords
                .Include(g => g.FirstAssistPlayer)
                .Include(g => g.Game)
                .Include(g => g.ScoringPlayer)
                .Include(g => g.SecondAssistPlayer)
                .FirstOrDefaultAsync(m => m.GoalRecordId == id);
            if (goalRecord == null)
            {
                return NotFound();
            }

            return View(goalRecord);
        }

        // GET: Goal/Create
        [Authorize]
        public IActionResult Create(int id)
        {
            var gameRecord = _context.Set<Game>().FirstOrDefault(o => o.GameId == id);
            if (gameRecord.Finalized != true)
            {
                ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                ViewData["GameId"] = gameRecord.GameId;
                ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                return View();
            }
            else
            {
                TempData["Message"] = "Finalized games cannot have Additional goals";
                return RedirectToAction("Index", "Scorekeeper");
            }
        }

        // POST: Goal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("GoalRecordId,GameId,ScoringPlayerId,FirstAssistPlayerId,SecondAssistPlayerId,Period,GameTime")] GoalRecord goalRecord)
        {
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == goalRecord.GameId);
            var scoringPlayerRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.ScoringPlayerId);
            var firstAssistRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.FirstAssistPlayerId);
            var secondAssistRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.SecondAssistPlayerId);
            if (firstAssistRecord != null && secondAssistRecord != null)
            {
                if (secondAssistRecord.TeamId != scoringPlayerRecord.TeamId || firstAssistRecord.TeamId != scoringPlayerRecord.TeamId)
                {
                    TempData["Message"] = "The scoring player and the assisting players must be on the same team";
                    ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                    ViewData["GameId"] = gameRecord.GameId;
                    ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                    ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                    return View();
                }
                if (scoringPlayerRecord.PlayerId == firstAssistRecord.PlayerId || scoringPlayerRecord.PlayerId == secondAssistRecord.PlayerId ||
                    firstAssistRecord.PlayerId == secondAssistRecord.PlayerId)
                {
                    TempData["Message"] = "The scoring player and the assisting players cannot be the same person";
                    ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                    ViewData["GameId"] = gameRecord.GameId;
                    ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                    ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                    return View();
                }
            }
            else
            {
                if(firstAssistRecord != null)
                {
                    if (firstAssistRecord.TeamId != scoringPlayerRecord.TeamId)
                    {
                        TempData["Message"] = "The scoring player and the assisting players must be on the same team";
                        ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["GameId"] = gameRecord.GameId;
                        ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        return View();
                    }
                    if (scoringPlayerRecord.PlayerId == firstAssistRecord.PlayerId)
                    {
                        TempData["Message"] = "The scoring player and the assisting players cannot be the same person";
                        ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["GameId"] = gameRecord.GameId;
                        ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        return View();
                    }
                }
                else if(secondAssistRecord != null)
                {
                    if (secondAssistRecord.TeamId != scoringPlayerRecord.TeamId)
                    {
                        TempData["Message"] = "The scoring player and the assisting players must be on the same team";
                        ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["GameId"] = gameRecord.GameId;
                        ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        return View();
                    }
                    if (scoringPlayerRecord.PlayerId == secondAssistRecord.PlayerId)
                    {
                        TempData["Message"] = "The scoring player and the assisting players cannot be the same person";
                        ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["GameId"] = gameRecord.GameId;
                        ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                        return View();
                    }
                }
            }

            if (goalRecord.Period > 4)
            {
                TempData["Message"] = "Periods cannot exceed 4 as that is Overtime";
                ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                ViewData["GameId"] = gameRecord.GameId;
                ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                return View();
            }
            //Update Game and Player tables ones goal is scored
            if (gameRecord == null)
            {
                return NotFound();
            }
            if(scoringPlayerRecord.TeamId == gameRecord.AwayTeamId)
            {
                gameRecord.AwayScoreTotal++;
            }
            else if (scoringPlayerRecord.TeamId == gameRecord.HomeTeamId)
            {
                gameRecord.HomeScoreTotal++;
            }
            if (scoringPlayerRecord.PlayerId == goalRecord.ScoringPlayerId)
            {
                scoringPlayerRecord.PointTotal++;
                scoringPlayerRecord.ScoreTotal++;
            }
            //check if first assist record is not null
            if (firstAssistRecord != null)
            {
                if (firstAssistRecord.PlayerId == goalRecord.FirstAssistPlayerId)
                {
                    firstAssistRecord.PointTotal++;
                    firstAssistRecord.AssistTotal++;
                }
            }
            //check if second assist record is not null
            if (secondAssistRecord != null)
            {
                if (secondAssistRecord.PlayerId == goalRecord.SecondAssistPlayerId)
                {
                    secondAssistRecord.PointTotal++;
                    secondAssistRecord.AssistTotal++;
                }
            }
            if (ModelState.IsValid)
            {

                try
                {
                    //Add Goal Record
                    _context.Add(goalRecord);
                    await _context.SaveChangesAsync();
                    //Add Goals to Game Record
                    _context.Add(gameRecord);
                    await _context.SaveChangesAsync();
                    //Add to Player Stats
                    _context.Add(scoringPlayerRecord);
                    await _context.SaveChangesAsync();

                    _context.Add(firstAssistRecord);
                    await _context.SaveChangesAsync();

                    _context.Add(secondAssistRecord);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Goal Information Created";
                    return RedirectToAction("Index","Scorekeeper");
                }
                TempData["Message"] = "Goal Information Not Created";
                return RedirectToAction("Index", "Scorekeeper");
            }
            ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
            ViewData["GameId"] = gameRecord.GameId;
            ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
            ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
            return View(goalRecord);
        }

        // GET: Goal/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goalRecord = await _context.GoalRecords.FindAsync(id);
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == goalRecord.GameId);
            if (goalRecord == null)
            {
                return NotFound();
            }
            if (gameRecord.Finalized != true)
            {
                ViewData["FirstAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                ViewData["GameId"] = gameRecord.GameId;
                ViewData["ScoringPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                ViewData["SecondAssistPlayerId"] = new SelectList(_context.Players.Where(x => x.TeamId == gameRecord.HomeTeamId || x.TeamId == gameRecord.AwayTeamId).Include(x => x.Team).OrderBy(x => x.Team), "PlayerId", "NameandNumber");
                return View(goalRecord);
            }
            else
            {
                TempData["Message"] = "Finalized games cannot have Additional goals";
                return RedirectToAction("Index", "Scorekeeper");
            }
        }

        // POST: Goal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("GoalRecordId,GameId,ScoringPlayerId,FirstAssistPlayerId,SecondAssistPlayerId,Period,GameTime")] GoalRecord goalRecord)
        {
            if (id != goalRecord.GoalRecordId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalRecordExists(goalRecord.GoalRecordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Goal Updated";
                return RedirectToAction("Index", "Scorekeeper");
            }
            TempData["Message"] = "Goal Not Updated";
            return RedirectToAction("Index", "Scorekeeper");
        }

        // GET: Goal/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goalRecord = await _context.GoalRecords
                .Include(g => g.FirstAssistPlayer)
                .Include(g => g.Game)
                .Include(g => g.ScoringPlayer)
                .Include(g => g.SecondAssistPlayer)
                .FirstOrDefaultAsync(m => m.GoalRecordId == id);
            if (goalRecord == null)
            {
                return NotFound();
            }

            return View(goalRecord);
        }

        // POST: Goal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goalRecord = await _context.GoalRecords.FindAsync(id);
            var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == goalRecord.GameId);
            var scoringPlayerRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.ScoringPlayerId);
            var firstAssistRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.FirstAssistPlayerId);
            var secondAssistRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.SecondAssistPlayerId);

            if (gameRecord == null)
            {
                return NotFound();
            }
            if (scoringPlayerRecord.TeamId == gameRecord.AwayTeamId)
            {
                gameRecord.AwayScoreTotal = gameRecord.AwayScoreTotal - 1;
            }
            else if (scoringPlayerRecord.TeamId == gameRecord.HomeTeamId)
            {
                gameRecord.HomeScoreTotal = gameRecord.HomeScoreTotal - 1;
            }
            if (scoringPlayerRecord.PlayerId == goalRecord.ScoringPlayerId)
            {
               
                scoringPlayerRecord.ScoreTotal = scoringPlayerRecord.ScoreTotal - 1;
                scoringPlayerRecord.PointTotal = scoringPlayerRecord.PointTotal - 1;
            }
            if (firstAssistRecord != null)
            {
                if (firstAssistRecord.PlayerId == goalRecord.FirstAssistPlayerId)
                {

                    firstAssistRecord.AssistTotal = firstAssistRecord.AssistTotal - 1;
                    firstAssistRecord.PointTotal = firstAssistRecord.PointTotal - 1;
                }
            }
            if (secondAssistRecord != null)
            {
                if (secondAssistRecord.PlayerId == goalRecord.SecondAssistPlayerId)
                {

                    secondAssistRecord.AssistTotal = secondAssistRecord.AssistTotal - 1;
                    secondAssistRecord.PointTotal = secondAssistRecord.PointTotal - 1;
                }
            }

            try
            {
                //Delete Goal
                _context.GoalRecords.Remove(goalRecord);
                await _context.SaveChangesAsync();
                //Subtract Goal from Game Record
                _context.Add(gameRecord);
                await _context.SaveChangesAsync();
                //Subtract to Player Stats
                _context.Add(scoringPlayerRecord);
                await _context.SaveChangesAsync();

                _context.Add(firstAssistRecord);
                await _context.SaveChangesAsync();

                _context.Add(secondAssistRecord);
                await _context.SaveChangesAsync();
               

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Goal Information Deleted";
                return RedirectToAction("Index", "Scorekeeper");
            }
            TempData["Message"] = "Goal Information Not Deleted";
            return RedirectToAction("Index", "Scorekeeper");
        }

        private bool GoalRecordExists(int id)
        {
            return _context.GoalRecords.Any(e => e.GoalRecordId == id);
        }
    }
}
