using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO.Response;

namespace FrontEnd.Pages.Admin.Sessions
{
    public class ListSessionsModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public ListSessionsModel(IApiClient apiClient)
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
                c.Sessions = c.Sessions.OrderBy(cs => cs.StartTime).ToList();
            });

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var session = await _apiClient.GetSessionAsync(id);

            if (session != null)
            {
                await _apiClient.DeleteSessionAsync(id);
            }

            return RedirectToPage();
        }
    }
}