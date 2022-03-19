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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PIHLSite.Models.ViewModel;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace PIHLSite.Controllers
{

    public class AdminController : Controller
    {
        private readonly PIHLDBContext _context;
        private readonly UserManager<PIHLSiteUser> userManager;



        public AdminController(PIHLDBContext context, UserManager<PIHLSiteUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            

        }



        // GET: Admin
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.AspNetUsers.ToListAsync());
        }

        // GET: Admin/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,FirstName,LastName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUser);
        }



        // GET: Admin/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            string adminUser = User.Identity.Name.ToString();
            if (adminUser == "christopher.thoms@colliers.com" || adminUser == "Christopher.Thoms@colliers.com")
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.PasswordHash,
                    ConfirmPassword = user.PasswordHash
                };
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                //byte[] salt;
                //new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                //var pbkdf2 = new Rfc2898DeriveBytes(model.Password, salt, 100000);
                //byte[] hash = pbkdf2.GetBytes(20);

                //byte[] hashBytes = new byte[36];
                //Array.Copy(salt, 0, hashBytes, 0, 16);
                //Array.Copy(hash, 0, hashBytes, 16, 20);

                //string savedPasswordHash = Convert.ToBase64String(hashBytes);

                var savedPasswordHash = Crypto.HashPassword(model.Password);

                user.Email = model.Email;
                user.UserName = model.UserName;
                user.PasswordHash = savedPasswordHash;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                try
                {
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return View(model);
                    }
                }
                catch (Exception)
                {
                    return NotFound();

                }
                return NotFound();
              
            }
        }

        // GET: Admin/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            _context.AspNetUsers.Remove(aspNetUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
