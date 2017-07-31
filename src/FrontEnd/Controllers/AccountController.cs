using FrontEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrontEnd.Controllers
{
    [Authorize] 
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger.ForContext<AccountController>();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.Information("User logged out.");
            return Redirect("/");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                // TODO: admned, can't find a way to pass error message to pages
                //HttpContext.Session.SetString("ExternalLoginError", $"Error from external provider: {remoteError}");
                _logger.Error(remoteError);

                return Redirect("/Account/Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Redirect("/Account/Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                _logger.Information("User logged in with {Name} provider.", info.LoginProvider);
                return Redirect("/");

            }
            if (signInResult.RequiresTwoFactor)
            {
                //HttpContext.Session.SetString("ExternalLoginError", "The external login require two factor validation");
                return Redirect("/Account/TwoFactor");
            }
            if (signInResult.IsLockedOut)
            {
                HttpContext.Session.SetString("ExternalLoginError", "This account has been locked out, please try again later.");
                return Redirect("/Account/Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new ApplicationUser { UserName = email, Email = email };
                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.Information("User created an account using {Name} provider.", info.LoginProvider);

                        return Redirect("/");
                    }
                }

                //HttpContext.Session.SetString("ExternalLoginError", "Unsuccessful login with service.");
                return Redirect("/Account/Login");
            }
        }
    }
}
