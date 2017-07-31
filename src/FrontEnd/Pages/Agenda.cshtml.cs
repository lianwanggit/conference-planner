using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConferenceDTO.Response;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using FrontEnd.Services;

namespace FrontEnd.Pages
{
    public class AgendaModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }

        public IEnumerable<(int Offset, DayOfWeek? DayofWeek)> DayOffsets { get; set; }

        public int ConferenceId { get; set; }
        public int CurrentDayOffset { get; set; }
        public string ConferenceName { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public AgendaModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }      

        public async Task OnGetAsync(string slug, int day = 0)
        {
            CurrentDayOffset = day;

            var conference = await _apiClient.GetConferenceAsync(slug);
            ConferenceId = conference.ID;
            ConferenceName = conference.Name;

            var sessions = await _apiClient.GetSessionsByConferenceIdAsync(ConferenceId);

            var startDate = sessions.Min(s => s.StartTime?.Date);
            var endDate = sessions.Max(s => s.EndTime?.Date);

            var numberOfDays = ((endDate - startDate)?.Days) + 1;

            DayOffsets = Enumerable.Range(0, numberOfDays ?? 0)
                .Select(offset => (offset, (startDate?.AddDays(offset))?.DayOfWeek));

            var filterDate = startDate?.AddDays(day);

            Sessions = sessions.Where(s => s.StartTime?.Date == filterDate)
                               .OrderBy(s => s.TrackId)
                               .GroupBy(s => s.StartTime)
                               .OrderBy(g => g.Key);
        }
    }
}