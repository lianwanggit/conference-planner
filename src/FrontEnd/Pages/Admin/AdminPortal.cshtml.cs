using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Models;
using FrontEnd.Services;
using System.Threading.Tasks;
using System.Linq;

namespace FrontEnd.Pages.Admin
{
    public class AdminPortalModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public AdminPortalModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public AdminPortalViewModel[] AdminPortalViewModels { get; set; }

        public async Task OnGetAsync()
        {
            var conferences = await _apiClient.GetConferencesAsync();
            var trackCount = conferences.Sum(c => c.Tracks?.Count ?? 0);
            var sessionCount = conferences.Sum(c => c.Sessions?.Count ?? 0);

            var speakers = await _apiClient.GetSpeakersAsync();
            var attendees = await _apiClient.GetAttendeesAsync();

            AdminPortalViewModels = new[] {
                new AdminPortalViewModel { PageName = "Conferences", EntityCount = conferences.Count, PageRoute = "./Conferences/ListConferences" },
                new AdminPortalViewModel { PageName = "Tracks", EntityCount = trackCount, PageRoute = "./Tracks/AddTrack" },
                new AdminPortalViewModel { PageName = "Sessions", EntityCount = sessionCount, PageRoute = "./Sessions/ListSessions" },
                new AdminPortalViewModel { PageName = "Speakers", EntityCount = speakers.Count, PageRoute = "./Speakers/AddSpeaker" },
                new AdminPortalViewModel { PageName = "Attendees", EntityCount = attendees.Count, PageRoute = "./Attendees/AddAttendee" },
            };
        }
    }
}