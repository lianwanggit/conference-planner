using ConferenceDTO.Response;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public class SessionModel : PageModel
    {
        private readonly IApiClient _apiClient;
        private readonly HtmlEncoder _htmlEncoder;

        public SessionResponse Session { get; set; }

        public int? DayOffset { get; set; }
        public bool IsInPersonalAgenda { get; set; }

        public SessionModel(IApiClient apiClient, HtmlEncoder htmlEncoder)
        {
            _apiClient = apiClient;
            _htmlEncoder = htmlEncoder;
        }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            Session = await _apiClient.GetSessionAsync(slug);

            if (Session == null)
            {
                return RedirectToPage("/Index");
            }

            var sessions = await _apiClient.GetSessionsByAttendeeAsync(User.Identity.Name);

            IsInPersonalAgenda = sessions.Any(s => s.ID == Session.ID);

            var allSessions = await _apiClient.GetSessionsAsync();

            var startDate = allSessions.Min(s => s.StartTime?.Date);

            DayOffset = Session.StartTime?.DateTime.Subtract(startDate ?? DateTime.MinValue).Days;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int sessionId)
        {
            await _apiClient.AddSessionToAttendeeAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int sessionId)
        {
            await _apiClient.RemoveSessionFromAttendeeAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }
    }
}