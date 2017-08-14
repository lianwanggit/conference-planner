using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd.Pages.Admin.Tracks
{
    public class EditTrackModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public EditTrackModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Track Track { get; set; }

        public Conference Conference { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGetAsync(int id)
        {
            var track = await _apiClient.GetTrackAsync(id);
            Track = new Track
            {
                TrackID = track.TrackID,
                ConferenceID = track.ConferenceID,
                Name = track.Name,
                Slug = track.Slug
            };

            Conference = track.Conference;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Conference = await _apiClient.GetConferenceAsync(Session.ConferenceID);

            //ValidateSessionTime();

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
    }
}