using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd.Pages.Admin.Sessions
{
    public class AddSessionModel : PageModel
    {

        private readonly IApiClient _apiClient;

        public AddSessionModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Session Session { get; set; }

        public string ConferenceName { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Session updated successfully!";

            await _apiClient.Add(Session);

            return RedirectToPage();
        }
    }
}