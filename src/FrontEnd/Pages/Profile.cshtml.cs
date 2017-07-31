using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontEnd.Services;

namespace FrontEnd.Pages
{
    public class ProfileModel : WelcomeModel
    {
        public ProfileModel(IApiClient apiClient, IImageProcessor imageProcessor) 
            : base(apiClient, imageProcessor)
        {
        }

        public async Task<IActionResult> OnGetAsync(string name)
        {
            Attendee = await _apiClient.GetAttendeeAsync(name);

            return Page();
        }

    }
}