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
            await GetConferences();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var conference = await _apiClient.GetConferenceAsync(Session.ConferenceID);
            if (conference == null)
            {
                await DisplayValidationError("Session.ConferenceID", "The selected conference is not available");
            }
            if (Session.StartTime.HasValue 
                && conference.StartDate.HasValue
                && Session.StartTime < conference.StartDate)
            {
                await DisplayValidationError("Session.StartTime", "The Session StartTime must be later than Conference StartDate");
            }
            if (conference.EndDate.HasValue
                && Session.EndTime.HasValue
                && Session.EndTime > conference.EndDate)
            {
                await DisplayValidationError("Session.EndTime", "The Session EndTime must be earlier than Conference EndDate");
            }
            if (Session.StartTime.HasValue
                && Session.EndTime.HasValue
                && Session.StartTime >= Session.EndTime)
            {
                await DisplayValidationError("Session.EndTime", "The Session EndTime must be later than StartTime");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Session created successfully!";

            await _apiClient.AddSessionAsync(Session);

            return RedirectToPage();
        }

        private async Task GetConferences()
        {
            var conferences = await _apiClient.GetConferencesAsync();
            Conferences = conferences.Select(c =>
                new SelectListItem { Value = c.ID.ToString(), Text = c.Name })
                .ToList();
        }

        private async Task DisplayValidationError(string key, string message)
        {
            await GetConferences();
            ModelState.AddModelError(key, message);
        }
    }
}