using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd.Pages.Admin.Tracks
{
    public class AddTrackModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public AddTrackModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Track Track { get; set; }

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
            var tracks = await _apiClient.GetTracksAsync();
            if (tracks.Any(t => t.Name == Track.Name))
            {
                ModelState.AddModelError("Track.Name", $"The track name {Track.Name} exists already");
            }

            if (!ModelState.IsValid)
            {
                await GetConferences();

                return Page();
            }

            Message = "Track created successfully!";

            await _apiClient.AddTrackAsync(Track);

            return RedirectToPage();
        }

        private async Task GetConferences()
        {
            var conferences = await _apiClient.GetConferencesAsync();
            Conferences = conferences.Select(c =>
                new SelectListItem { Value = c.ID.ToString(), Text = c.Name })
                .ToList();
        }
    }
}