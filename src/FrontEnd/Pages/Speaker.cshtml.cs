using ConferenceDTO.Response;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public class SpeakerModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public SpeakerModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public SpeakerResponse Speaker { get; set; }

        public async Task<IActionResult> OnGet(string slug)
        {
            Speaker = await _apiClient.GetSpeakerAsync(slug);

            if (Speaker == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}