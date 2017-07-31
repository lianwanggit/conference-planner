using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public class WelcomeModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        protected readonly IImageProcessor _imageProcessor;

        public WelcomeModel(IApiClient apiClient, IImageProcessor imageProcessor)
        {
            _apiClient = apiClient;
            _imageProcessor = imageProcessor;
        }

        [BindProperty]
        public Attendee Attendee { get; set; }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> files)
        {
            if (files.Count == 1 && files[0].Length > 0)
            {
                var file = files[0];

                var avatar = new ConferenceDTO.File
                {
                    FileName = Path.GetFileName(file.FileName),
                    ContentType = "image/png" //file.ContentType
                };

                MemoryStream memoryStream;    
                using (var inputStream = file.OpenReadStream())
                {
                    _imageProcessor.GenerateAvatar(inputStream, 
                        new SixLabors.Primitives.Size(200, 200), 
                        100, 
                        out memoryStream);

                    avatar.Content = memoryStream.ToArray();
                    memoryStream.Dispose();
                }

                Attendee.Avatar = avatar;
            }

            await _apiClient.AddAttendeeAsync(Attendee);

            return RedirectToPage("/Index");
        }
    }
}