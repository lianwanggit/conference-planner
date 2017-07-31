using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd.Pages.Admin
{
    public class EditSessionModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public EditSessionModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Session Session { get; set; }

        public Conference Conference { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGetAsync(int id)
        {
            var session = await _apiClient.GetSessionAsync(id);
            Session = new Session
            {
                ID = session.ID,
                ConferenceID = session.ConferenceID,
                TrackId = session.TrackId,
                Title = session.Title,
                Slug = session.Slug,
                Abstract = session.Abstract,
                StartTime = session.StartTime,
                EndTime = session.EndTime
            };

            Conference = session.Conference;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Conference = await _apiClient.GetConferenceAsync(Session.ConferenceID);

            ValidateSessionTime();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Session updated successfully!";

            await _apiClient.PutSessionAsync(Session);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var session = await _apiClient.GetSessionAsync(id);

            if (session != null)
            {
                await _apiClient.DeleteSessionAsync(id);
            }

            Message = "Session deleted successfully!";

            return RedirectToPage("/Index");
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