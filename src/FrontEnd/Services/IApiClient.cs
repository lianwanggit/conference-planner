using ConferenceDTO;
using ConferenceDTO.Response;
using ConferenceDTO.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontEnd.Services
{
    public interface IApiClient
    {
        Task<List<ConferenceResponse>> GetConferencesAsync();
        Task<ConferenceResponse> GetConferenceAsync(int id);
        Task<ConferenceResponse> GetConferenceAsync(string slug);
        Task AddConferenceAsync(Conference conference);
        Task PutConferenceAsync(Conference conference);
        Task DeleteConferenceAsync(int id);

        Task<List<SessionResponse>> GetSessionsAsync();
        Task<List<SessionResponse>> GetSessionsByConferenceIdAsync(int id);
        Task<SessionResponse> GetSessionAsync(int id);
        Task<SessionResponse> GetSessionAsync(string slug);
        Task AddSessionAsync(Session session);
        Task PutSessionAsync(Session session);
        Task DeleteSessionAsync(int id);

        Task<List<SpeakerResponse>> GetSpeakersAsync();
        Task<SpeakerResponse> GetSpeakerAsync(int id);
        Task<SpeakerResponse> GetSpeakerAsync(string slug);

        Task<List<SearchResult>> SearchAsync(string query);

        Task<List<AttendeeResponse>> GetAttendeesAsync();
        Task<AttendeeResponse> GetAttendeeAsync(string name);
        Task AddAttendeeAsync(Attendee attendee);
        Task<List<SessionResponse>> GetSessionsByAttendeeAsync(string name);
        Task AddSessionToAttendeeAsync(string name, int sessionId);
        Task RemoveSessionFromAttendeeAsync(string name, int sessionId);

        Task<List<TrackResponse>> GetTracksAsync();
        Task<TrackResponse> GetTrackAsync(int id);
        Task<TrackResponse> GetTrackAsync(string slug);
        Task AddTrackAsync(Track track);
        Task PutTrackAsync(Track track);
        Task DeleteTrackAsync(int id);

        Task<File> GetFileAsync(int id);
    }
}
