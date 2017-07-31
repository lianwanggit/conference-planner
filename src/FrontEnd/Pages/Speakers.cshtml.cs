using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontEnd.Services;
using ConferenceDTO.Response;

namespace FrontEnd.Pages
{
    public class SpeakersModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public SpeakersModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IEnumerable<SpeakerResponse> Speakers { get; set; }

        public async Task OnGet()
        {
            var speakers = await _apiClient.GetSpeakersAsync();

            Speakers = speakers.OrderBy(s => s.Name);
        }
    }
}