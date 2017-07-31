using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace FrontEnd.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager,
            ILogger logger)
        {
            _userManager = userManager;
            _logger = logger.ForContext<ResetPasswordModel>();
        }

        [BindProperty]
        public ResetPasswordViewModel ResetPasswordViewModel { get; set; }

        public void OnGet(string code)
        {
            ResetPasswordViewModel = new ResetPasswordViewModel
            {
                Code = code
            };
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(ResetPasswordViewModel.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, ResetPasswordViewModel.Code, ResetPasswordViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("ResetPasswordConfirmation");
            }

            AddErrors(result);

            return Page();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}