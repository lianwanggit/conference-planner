using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin.Conferences
{
    public class EditConferenceModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public EditConferenceModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Conference Conference { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Conference = await _apiClient.GetConferenceAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Conference changed successfully!";

            await _apiClient.PutConferenceAsync(Conference);

            return RedirectToPage();
        }
    }
}