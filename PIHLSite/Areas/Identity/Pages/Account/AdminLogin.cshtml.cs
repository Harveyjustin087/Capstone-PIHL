using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PIHLSite.Areas.Identity.Data;
using System.Net;

namespace PIHLSite.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class AdminLoginModel : PageModel
    {
        private readonly UserManager<PIHLSiteUser> _userManager;
        private readonly SignInManager<PIHLSiteUser> _signInManager;
        private readonly ILogger<AdminLoginModel> _logger;

        public AdminLoginModel(SignInManager<PIHLSiteUser> signInManager, 
            ILogger<AdminLoginModel> logger,
            UserManager<PIHLSiteUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
  

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        
        public bool ReCaptchaPassed(string gRecaptchaResponse)
        {

            using (var webclient = new WebClient())
            {
                string validateString = "https://www.google.com/recaptcha/api/siteverify?secret=" + "6LdisQEdAAAAAF3f8d3eNhDJ0tTzRxZzz4r1bx1d" + "&response=" + gRecaptchaResponse;
                var recaptcha_result = webclient.DownloadString(validateString);
                if (recaptcha_result.ToLower().Contains("false"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                string recaptchaResponse = this.Request.Form["g-recaptcha-response"];
                if (!ReCaptchaPassed(recaptchaResponse))
                {
                    ModelState.AddModelError("", "You failed the Captcha please try again");
                    return Page();
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
