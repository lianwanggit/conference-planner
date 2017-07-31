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

        public Conference Conference { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGetAsync()
        {
            await GetConferences();
        }

        public async Task<IActionResult> OnPostAsync()
        {
           Conference = await _apiClient.GetConferenceAsync(Session.ConferenceID);
            ValidateSessionTime();

            if (!ModelState.IsValid)
            {
                await GetConferences();

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

        private void ValidateSessionTime()
        {
            if (Conference == null)
            {
                ModelState.AddModelError("Session.ConferenceID", "The selected conference is not available");
            }
            if (Session.StartTime.HasValue
                && Conference.StartDate.HasValue
                && Session.StartTime < Conference.StartDate)
            {
                ModelState.AddModelError("Session.StartTime", "The Session StartTime must be later than Conference StartDate");
            }
            if (Session.StartTime.HasValue
                && Conference.EndDate.HasValue
                && Session.StartTime > Conference.EndDate)
            {
                ModelState.AddModelError("Session.StartTime", "The Session StartTime must be earlier than Conference EndDate");
            }
            if (Conference.EndDate.HasValue
                && Session.EndTime.HasValue
                && Session.EndTime > Conference.EndDate)
            {
                ModelState.AddModelError("Session.EndTime", "The Session EndTime must be earlier than Conference EndDate");
            }
            if (Conference.StartDate.HasValue
                && Session.EndTime.HasValue
                && Session.EndTime < Conference.StartDate)
            {
                ModelState.AddModelError("Session.EndTime", "The Session EndTime must be later than Conference StartDate");
            }
            if (Session.StartTime.HasValue
                && Session.EndTime.HasValue
                && Session.StartTime >= Session.EndTime)
            {
                ModelState.AddModelError("Session.EndTime", "The Session EndTime must be later than StartTime");
            }
        }

    }
}