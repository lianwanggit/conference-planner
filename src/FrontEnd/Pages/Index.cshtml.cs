using ConferenceDTO.Response;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApiClient _apiClient;        

        public IndexModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IEnumerable<ConferenceResponse> UpcommingConferences { get; set; }

        public async Task OnGet(int day = 0)
        {
            var conferences = await _apiClient.GetConferencesAsync();

            UpcommingConferences = conferences
                                        .Where(s => s.StartDate?.ToUniversalTime() >= DateTimeOffset.UtcNow.Date
                                            && s.StartDate?.ToUniversalTime() <= DateTimeOffset.UtcNow.Date.AddDays(5))
                                        .OrderBy(s => s.StartDate);
        }

    }
}
