using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PIHLSite.Models;

namespace PIHLSite.Controllers
{
    public class TeamController : Controller
    {
        private readonly PIHLDBContext _context;

        public TeamController(PIHLDBContext context)
        {
            _context = context;
        }

        // GET: Team
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string adminUser = User.Identity.Name.ToString();
            if (adminUser == "christopher.thoms@colliers.com" || adminUser == "Christopher.Thoms@colliers.com")
            {
                var pIHLDBContext = _context.Teams.Include(t => t.Season);
                return View(await pIHLDBContext.ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Team/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Season)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Team/Create
        [Authorize]
        public IActionResult Create()
        {
            string adminUser = User.Identity.Name.ToString();
            if (adminUser == "christopher.thoms@colliers.com" || adminUser == "Christopher.Thoms@colliers.com")
            {
                ViewData["SeasonId"] = new SelectList(_context.Seasons, "SeasonId", "SeasonId");
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Team/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TeamId,Name,Company,Win,Loss,Otl,SeasonId")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "SeasonId", "SeasonId", team.SeasonId);
            return View(team);
        }

        // GET: Team/Edit/5
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

                var team = await _context.Teams.FindAsync(id);
                if (team == null)
                {
                    return NotFound();
                }
                ViewData["SeasonId"] = new SelectList(_context.Seasons, "SeasonId", "SeasonId", team.SeasonId);
                return View(team);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Team/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,Name,Company,Win,Loss,Otl,SeasonId")] Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
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
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "SeasonId", "SeasonId", team.SeasonId);
            return View(team);
        }

        // GET: Team/Delete/5
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

                var team = await _context.Teams
                    .Include(t => t.Season)
                    .FirstOrDefaultAsync(m => m.TeamId == id);
                if (team == null)
                {
                    return NotFound();
                }

                return View(team);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Team
        public void DownloadData()
        {
            var collection = _context.Teams.Include(t => t.Season);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("TeamsReport");
            Sheet.Cells["A1"].Value = "Team Name";
            Sheet.Cells["B1"].Value = "Win";
            Sheet.Cells["C1"].Value = "Loss";
            Sheet.Cells["D1"].Value = "OTL";
            Sheet.Cells["E1"].Value = "Season";
            int row = 2;
            foreach (var item in collection)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = item.Name;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Win;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Loss;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Otl;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.Season.StartYear + " - " + item.Season.EndYear;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "TeamsReport.xlsx");
            Response.Body.WriteAsync(Ep.GetAsByteArray());
            Response.CompleteAsync();
            
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.TeamId == id);
        }
    }
}