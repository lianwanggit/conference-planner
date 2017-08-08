using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO.Response;

namespace FrontEnd.Pages.Admin.Tracks
{
    public class ListTracksModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public ListTracksModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

        }

        public List<ConferenceResponse> Conferences { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var conferences = await _apiClient.GetConferencesAsync();
            Conferences = conferences.OrderByDescending(c => c.StartDate).ToList();

            Conferences.ForEach(c =>
            {
                c.Sessions = c.Sessions.OrderBy(cs => cs.Title)
                                .ThenBy(cs => cs.StartTime)
                                .ToList();
            });

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var track = await _apiClient.GetTrackAsync(id);

            if (track != null)
            {
                await _apiClient.DeleteTrackAsync(id);
            }

            return RedirectToPage();
        }
    }
}