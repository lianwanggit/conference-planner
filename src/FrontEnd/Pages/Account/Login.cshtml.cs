using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using FrontEnd.Models;
using Serilog;

namespace FrontEnd.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenticationSchemeProvider _authSchemeProvider;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public LoginModel(IAuthenticationSchemeProvider authSchemeProvider, 
            SignInManager<ApplicationUser> signInManager,
            ILogger logger)
        {
            _authSchemeProvider = authSchemeProvider;
            _signInManager = signInManager;
            _logger = logger.ForContext<LoginModel>();
        }

        public IEnumerable<AuthenticationScheme> AuthSchemes { get; set; } = new List<AuthenticationScheme>();

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            AuthSchemes = await _authSchemeProvider.GetRequestHandlerSchemesAsync();

            return Page();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostExternalLoginAsync(string scheme)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);
            return Challenge(properties, scheme);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostLoginAsync()
        {
            AuthSchemes = await _authSchemeProvider.GetRequestHandlerSchemesAsync();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(LoginViewModel.Email, 
                    LoginViewModel.Password, 
                    LoginViewModel.RememberMe, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.Information("User logged in.");
                    return RedirectToPage("/Index");
                }
                if (result.RequiresTwoFactor)
                {
                    ModelState.AddModelError(string.Empty, "The external login require two factor validation.");
                    return RedirectToPage("TwoFactor");
                    //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.Warning("User account locked out.");
                    return RedirectToPage("Lockout");
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