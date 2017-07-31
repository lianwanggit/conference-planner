using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Models;

namespace FrontEnd.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty]
        public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; }

        public void OnGet()
        {
        }
    }
}