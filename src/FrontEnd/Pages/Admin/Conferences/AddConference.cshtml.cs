using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin.Conferences
{
    public class AddConferenceModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public AddConferenceModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Conference Conference { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Conference created successfully!";

            await _apiClient.AddConferenceAsync(Conference);

            return RedirectToPage();
        }
    }
}