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

        public List<TrackResponse> Tracks { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var tracks = await _apiClient.GetTracksAsync();
            Tracks = tracks.OrderBy(c => c.Name).ToList();

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