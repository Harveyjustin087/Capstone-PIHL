using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using PIHLSite.Models;

namespace PIHLSite.Controllers
{
    public class ScorekeeperController : Controller
    {
        private readonly PIHLDBContext _context;

        public ScorekeeperController(PIHLDBContext context)
        {
            _context = context;
        }
     
        
        // GET: Scorekeeper
        public async Task<IActionResult> Index()
        {
            var pihlContext = _context.Games.Include(o => o.HomeTeam).Include(o => o.AwayTeam);
            return View(await pihlContext.ToListAsync());
        }

        // GET: Scorekeeper/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.Include(o => o.HomeTeam).Include(o => o.AwayTeam)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Scorekeeper/Create
        public IActionResult Create()
        {
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "AwayTeamId", "Name");
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "HomeTeamId", "Name");
            return View();
        }

        // POST: Scorekeeper/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,GameDate,HomeScoreTotal,AwayScoreTotal,AwayTeamId,HomeTeamId,Finalized,Overtime")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "AwayTeamId", "Name");
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "HomeTeamId", "Name");
            return View(game);
        }
       
        // GET: Scorekeeper/Edit/5
        public async Task<IActionResult> Finalize(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.Include(o => o.HomeTeam).Include(o => o.AwayTeam)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["AwayTeam.Name"] = new SelectList(_context.Teams, "TeamId", "Name", game.AwayTeamId);
            ViewData["HomeTeam.Name"] = new SelectList(_context.Teams, "TeamId", "Name", game.HomeTeamId);
            return View(game);
        }

        // POST: Scorekeeper/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalize(int id, [Bind("GameId,GameDate,HomeScoreTotal,OppScoreTotal,AwayTeamId,HomeTeamId,Finalized,Overtime")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var gameRecord = await _context.Set<Game>().FirstOrDefaultAsync(o => o.GameId == game.GameId);
                var awayTeamRecord = await _context.Set<Team>().FirstOrDefaultAsync(o => o.TeamId == game.AwayTeamId);
                var homeTeamRecord = await _context.Set<Team>().FirstOrDefaultAsync(o => o.TeamId == game.HomeTeamId);
                if (gameRecord == null)
                {
                    return NotFound();
                }
                gameRecord.Finalized = true;
                if (awayTeamRecord.TeamId == gameRecord.AwayTeamId)
                {
                    if (gameRecord.Overtime == false)
                    {
                        if (gameRecord.AwayScoreTotal > gameRecord.HomeScoreTotal)
                        {
                            awayTeamRecord.Win++;
                            homeTeamRecord.Loss++;
                        }
                        else if(gameRecord.AwayScoreTotal < gameRecord.HomeScoreTotal)
                        {
                            awayTeamRecord.Loss++;
                            homeTeamRecord.Win++;
                        }
                        else
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        if (gameRecord.AwayScoreTotal > gameRecord.HomeScoreTotal)
                        {
                            awayTeamRecord.Win++;
                            homeTeamRecord.Otl++;
                        }
                        else
                        {
                            awayTeamRecord.Otl++;
                            homeTeamRecord.Win++;
                        }
                    }
                }
                try
                {
                    _context.Update(gameRecord);
                    await _context.SaveChangesAsync();

                    _context.Update(homeTeamRecord);
                    await _context.SaveChangesAsync();

                    _context.Update(awayTeamRecord);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "AwayTeamId", "Name", game.AwayTeam.TeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "HomeTeamId", "Name", game.HomeTeam.TeamId);
            return View(game);
        }
        // GET: Scorekeeper/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Scorekeeper/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,GameDate,HomeScoreTotal,OppScoreTotal,AwayTeamId,HomeTeamId,Finalized,Overtime")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            return View(game);
        }

        // GET: Scorekeeper/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Scorekeeper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameRecord = await _context.Games.FindAsync(id);
            var awayTeamRecord = await _context.Set<Team>().FirstOrDefaultAsync(o => o.TeamId == gameRecord.AwayTeamId);
            var homeTeamRecord = await _context.Set<Team>().FirstOrDefaultAsync(o => o.TeamId == gameRecord.HomeTeamId);
            var penaltyRecord = await _context.Set<PenaltyRecord>().FirstOrDefaultAsync(o => o.GameId == gameRecord.GameId);

            if (gameRecord == null)
            {
                return NotFound();
            }
            //Readjust Player Stats
            var goalRecord = await _context.Set<GoalRecord>().FirstOrDefaultAsync(o => o.GameId == gameRecord.GameId);
            if (goalRecord != null)
            {
                var scoringPlayerRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.ScoringPlayerId);
                var firstAssistRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.FirstAssistPlayerId);
                var secondAssistRecord = await _context.Set<Player>().FirstOrDefaultAsync(o => o.PlayerId == goalRecord.SecondAssistPlayerId);
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
                if (firstAssistRecord.PlayerId == goalRecord.FirstAssistPlayerId)
                {

                    firstAssistRecord.AssistTotal = firstAssistRecord.AssistTotal - 1;
                    firstAssistRecord.PointTotal = firstAssistRecord.PointTotal - 1;
                }
                if (secondAssistRecord.PlayerId == goalRecord.SecondAssistPlayerId)
                {

                    secondAssistRecord.AssistTotal = secondAssistRecord.AssistTotal - 1;
                    secondAssistRecord.PointTotal = secondAssistRecord.PointTotal - 1;
                }
                try
                {
                    //Update Player Stats
                    _context.Add(scoringPlayerRecord);
                    await _context.SaveChangesAsync();

                    _context.Add(firstAssistRecord);
                    await _context.SaveChangesAsync();

                    _context.Add(secondAssistRecord);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {

                }
            }
            //Readjust Team Stats
            if (gameRecord.Overtime == false)
            {
                if (gameRecord.AwayScoreTotal > gameRecord.HomeScoreTotal)
                {
                    awayTeamRecord.Win--;
                    homeTeamRecord.Loss--;
                }
                else if (gameRecord.AwayScoreTotal < gameRecord.HomeScoreTotal)
                {
                    awayTeamRecord.Loss--;
                    homeTeamRecord.Win--;
                }
            }
            else
            {
                if (gameRecord.AwayScoreTotal > gameRecord.HomeScoreTotal)
                {
                    awayTeamRecord.Win--;
                    homeTeamRecord.Otl--;
                }
                else
                {
                    awayTeamRecord.Otl--;
                    homeTeamRecord.Win--;
                }
            }
            try
            {
               
                //Delete Goals
                if (goalRecord != null)
                {
                    _context.GoalRecords.Where(o => o.GameId == goalRecord.GameId)
                              .ToList().ForEach(o => _context.Remove(o));
                    await _context.SaveChangesAsync();
                }
                //Delete Penalty Records
                if (penaltyRecord != null)
                {
                    _context.PenaltyRecords.Where(o => o.GameId == penaltyRecord.GameId)
                              .ToList().ForEach(o => _context.Remove(o));
                    await _context.SaveChangesAsync(); 
                }
                //Delete Game
                _context.Remove(gameRecord);
                await _context.SaveChangesAsync();

                //Update Team Stats
                _context.Add(homeTeamRecord);
                await _context.SaveChangesAsync();

                _context.Add(awayTeamRecord);
                await _context.SaveChangesAsync();


            }
            catch (Exception ex)
            {

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
}
