using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PIHLSite.Areas.Identity.Data;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.ExternalLoginModel;

namespace PIHLSite.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<PIHLSiteUser> _userManager;
        private readonly SignInManager<PIHLSiteUser> _signInManager;
        private readonly ILogger<AdminLoginModel> _logger;
        private readonly IEmailSender _sender;

        public ForgotPasswordModel(SignInManager<PIHLSiteUser> signInManager,
           ILogger<AdminLoginModel> logger,
           UserManager<PIHLSiteUser> userManager,
           IEmailSender sender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _sender = sender;
        }
        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }
        [BindProperty]
        public ForgotPasswordViewModel Reset { get; set; }
        public string ReturnUrl { get; set; }


        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

        }

        public async Task<ActionResult> OnPostAsync(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Page();
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Identity/Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _sender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return Redirect("/Identity/Account/ForgotPasswordConfirm");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
