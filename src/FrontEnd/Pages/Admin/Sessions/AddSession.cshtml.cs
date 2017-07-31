using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public List<SelectListItem> Conferences { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGetAsync()
        {
            var conferences = await _apiClient.GetConferencesAsync();
            Conferences = conferences.Select(c => 
                new SelectListItem { Value = c.ID.ToString(), Text = c.Name })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Session created successfully!";

            await _apiClient.AddSessionAsync(Session);

            return RedirectToPage();
        }
    }
}