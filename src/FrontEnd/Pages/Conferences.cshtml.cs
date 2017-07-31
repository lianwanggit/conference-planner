using ConferenceDTO.Response;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public class ConferencesModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public ConferencesModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IEnumerable<ConferenceResponse> Conferences { get; set; }

        public async Task OnGet(int day = 0)
        {
            var conferences = await _apiClient.GetConferencesAsync();

            Conferences = conferences.OrderByDescending(c => c.StartDate);
        }
    }
}