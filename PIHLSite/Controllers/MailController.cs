using Microsoft.AspNetCore.Mvc;
using PIHLSite.Models;
using PIHLSite.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PIHLSite.Areas.Identity.Data;
using PIHLSite.Areas.Identity.Pages.Account;
using PIHLSite.Models.ViewModel;
using System.Web.Helpers;

namespace PIHLSite.Controllers
{
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;
        private readonly UserManager<PIHLSiteUser> _userManager;
        private readonly SignInManager<PIHLSiteUser> _signInManager;
        public MailController(IMailService mailService, 
            SignInManager<PIHLSiteUser> signInManager, 
            UserManager<PIHLSiteUser> userManager)
        {
            this.mailService = mailService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.ToEmail);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Redirect("/Identity/Account/ForgotPasswordConfirm");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Mail", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                request.Body = $"Please reset your password by clicking here: '{callbackUrl}' ";
                request.Subject = "PIHL Password Reset Email";
                request.Attachments = null;
                await mailService.SendEmailAsync(request);
                return Redirect("/Identity/Account/ForgotPasswordConfirm");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult ResetPassword()
        {
            return Redirect("/Identity/Account/ResetPassword");
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel.ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Identity/Account/ResetPassword");
            }
            var user = await _userManager.FindByNameAsync(reset.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("/Identity/Account/ResetPasswordConfirmation");
            }
            else
            {
                var savedPasswordHash = Crypto.HashPassword(reset.Password);
                user.PasswordHash = savedPasswordHash;
                try
                {
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Redirect("/Identity/Account/ResetPasswordConfirmation");
                    }
                }
                catch(Exception e)
                {
                    return NotFound();

                }
            }
           
            return Redirect("/Identity/Account/ResetPasswordConfirmation");
        }

    }
}
