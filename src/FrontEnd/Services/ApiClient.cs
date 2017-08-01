using ConferenceDTO;
using ConferenceDTO.Response;
using ConferenceDTO.Search;
using FrontEnd.Infrastructure;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private ILogger _logger;

        public ApiClient(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger.ForContext<ApiClient>();
        }

        #region Conference

        public async Task<List<ConferenceResponse>> GetConferencesAsync()
        {
            _logger.Information("Http request - GetConferencesAsync.");

            var response = await _httpClient.GetAsync("/api/conferences");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<ConferenceResponse>>();
        }

        public async Task<ConferenceResponse> GetConferenceAsync(int id)
        {
            _logger.Information("Http request - GetConferenceAsync. id: {id}", id);

            var response = await _httpClient.GetAsync($"/api/conferences/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<ConferenceResponse>();
        }

        public async Task<ConferenceResponse> GetConferenceAsync(string slug)
        {
            _logger.Information("Http request - GetConferenceAsync. slug: {slug}", slug);

            var response = await _httpClient.GetAsync($"/api/Conferences/GetConferenceBySlug/{slug}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<ConferenceResponse>();
        }

        public async Task AddConferenceAsync(Conference conference)
        {
            _logger.Information("Http request - AddConferenceAsync. conference: {conference}", conference);

            var response = await _httpClient.PostJsonAsync($"/api/conferences", conference);

            response.EnsureSuccessStatusCode();
        }

        public async Task PutConferenceAsync(Conference conference)
        {
            _logger.Information("Http request - PutConferenceAsync. conference: {conference}", conference);

            var response = await _httpClient.PutJsonAsync($"/api/conferences/{conference.ID}", conference);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteConferenceAsync(int id)
        {
            _logger.Information("Http request - DeleteConferenceAsync. id: {id}", id);

            var response = await _httpClient.DeleteAsync($"/api/conferences/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region Session

        public async Task<List<SessionResponse>> GetSessionsAsync()
        {
            _logger.Information("Http request - GetSessionsAsync");

            var response = await _httpClient.GetAsync("/api/sessions");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<SessionResponse>>();
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            _logger.Information("Http request - GetSessionAsync. id: {id}", id);

            var response = await _httpClient.GetAsync($"/api/sessions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<SessionResponse>();
        }

        public async Task<SessionResponse> GetSessionAsync(string slug)
        {
            _logger.Information("Http request - GetSessionAsync. slug: {slug}", slug);

            var response = await _httpClient.GetAsync($"/api/Sessions/GetSessionBySlug/{slug}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<SessionResponse>();
        }

        public async Task<List<SessionResponse>> GetSessionsByConferenceIdAsync(int id)
        {
            _logger.Information("Http request - GetSessionsAsync");

            var response = await _httpClient.GetAsync($"/api/Sessions/GetSessionByConferenceId/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<SessionResponse>>();
        }

        public async Task AddSessionAsync(Session session)
        {
            _logger.Information("Http request - AddSessionAsync. session: {session}", session);

            var response = await _httpClient.PostJsonAsync($"/api/sessions/", session);

            response.EnsureSuccessStatusCode();
        }

        public async Task PutSessionAsync(Session session)
        {
            _logger.Information("Http request - PutSessionAsync. session: {session}", session);

            var response = await _httpClient.PutJsonAsync($"/api/sessions/{session.ID}", session);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteSessionAsync(int id)
        {
            _logger.Information("Http request - DeleteSessionAsync. id: {id}", id);

            var response = await _httpClient.DeleteAsync($"/api/sessions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region Speaker

        public async Task<List<SpeakerResponse>> GetSpeakersAsync()
        {
            _logger.Information("Http request - GetSpeakersAsync");

            var response = await _httpClient.GetAsync("/api/speakers");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<SpeakerResponse>>();
        }

        public async Task<SpeakerResponse> GetSpeakerAsync(int id)
        {
            _logger.Information("Http request - GetSpeakerAsync. id: {id}", id);

            var response = await _httpClient.GetAsync($"/api/speakers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<SpeakerResponse>();
        }

        public async Task<SpeakerResponse> GetSpeakerAsync(string slug)
        {
            _logger.Information("Http request - GetSpeakerAsync. slug: {slug}", slug);

            var response = await _httpClient.GetAsync($"/api/Speakers/GetSpeakerBySlug/{slug}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<SpeakerResponse>();
        }

        #endregion

        #region Attendee

        public async Task<List<AttendeeResponse>> GetAttendeesAsync()
        {
            _logger.Information("Http request - GetAttendeesAsync");

            var response = await _httpClient.GetAsync("/api/attendees");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<AttendeeResponse>>();
        }

        public async Task AddAttendeeAsync(Attendee attendee)
        {
            _logger.Information("Http request - AddAttendeeAsync. attendee: {attendee}", attendee);

            var response = await _httpClient.PostJsonAsync($"/api/attendees", attendee);

            response.EnsureSuccessStatusCode();
        }

        public async Task<AttendeeResponse> GetAttendeeAsync(string name)
        {
            _logger.Information("Http request - GetAttendeeAsync. name: {name}", name);

            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"/api/attendees/{name}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<AttendeeResponse>();
        }

        public async Task AddSessionToAttendeeAsync(string name, int sessionId)
        {
            _logger.Information("Http request - AddSessionToAttendeeAsync. name: {name}, sessionId: {sessionId}", name, sessionId);

            var response = await _httpClient.PostAsync($"/api/attendees/{name}/session/{sessionId}", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveSessionFromAttendeeAsync(string name, int sessionId)
        {
            _logger.Information("Http request - RemoveSessionFromAttendeeAsync. name: {name}", name);

            var response = await _httpClient.DeleteAsync($"/api/attendees/{name}/session/{sessionId}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<SessionResponse>> GetSessionsByAttendeeAsync(string name)
        {
            _logger.Information("Http request - GetSessionsByAttendeeAsync. name: {name}", name);

            // TODO: Add backend API for this

            var sessionsTask = GetSessionsAsync();
            var attendeeTask = GetAttendeeAsync(name);

            await Task.WhenAll(sessionsTask, attendeeTask);

            var sessions = await sessionsTask;
            var attendee = await attendeeTask;

            if (attendee == null)
            {
                return new List<SessionResponse>();
            }

            var sessionIds = attendee.Sessions.Select(s => s.ID);

            sessions.RemoveAll(s => !sessionIds.Contains(s.ID));

            return sessions;
        }

        #endregion

        #region Track

        public async Task<List<TrackResponse>> GetTracksAsync()
        {
            _logger.Information("Http request - GetTracksAsync.");

            var response = await _httpClient.GetAsync("/api/tracks");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<TrackResponse>>();
        }

        public async Task<TrackResponse> GetTrackAsync(int id)
        {
            _logger.Information("Http request - GetTrackAsync. id: {id}", id);

            var response = await _httpClient.GetAsync($"/api/tracks/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<TrackResponse>();
        }

        public async Task<TrackResponse> GetTrackAsync(string slug)
        {
            _logger.Information("Http request - GetTrackAsync. slug: {slug}", slug);

            var response = await _httpClient.GetAsync($"/api/tracks/GetTrackBySlug/{slug}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<TrackResponse>();
        }

        public async Task AddTrackAsync(Track track)
        {
            _logger.Information("Http request - AddTrackAsync. track: {track}", track);

            var response = await _httpClient.PostJsonAsync($"/api/tracks", track);

            response.EnsureSuccessStatusCode();
        }

        public async Task PutTrackAsync(Track track)
        {
            _logger.Information("Http request - PutTrackAsync. track: {track}", track);

            var response = await _httpClient.PutJsonAsync($"/api/tracks/{track.TrackID}", track);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTrackAsync(int id)
        {
            _logger.Information("Http request - DeleteTrackAsync. id: {id}", id);

            var response = await _httpClient.DeleteAsync($"/api/tracks/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        #endregion

        public async Task<List<SearchResult>> SearchAsync(string query)
        {
            _logger.Information("Http request - SearchAsync. query: {query}", query);

            var term = new SearchTerm
            {
                Query = query
            };

            var response = await _httpClient.PostJsonAsync($"/api/search", term);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<List<SearchResult>>();
        }

        public async Task<File> GetFileAsync(int id)
        {
            _logger.Information("Http request - GetFileAsync. id: {id}", id);

            var response = await _httpClient.GetAsync($"/api/files/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<File>();
        }

    }
}
