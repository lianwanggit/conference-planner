using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Pages.Admin.Conferences
{
    public class ListConferencesModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public ListConferencesModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

        }

        public List<ConferenceResponse> Conferences { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var conferences = await _apiClient.GetConferencesAsync();
            Conferences = conferences.OrderByDescending(c => c.StartDate).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var conference = await _apiClient.GetConferenceAsync(id);

            if (conference != null)
            {
                await _apiClient.DeleteConferenceAsync(id);
            }
           
            return RedirectToPage();
        }
    }
}