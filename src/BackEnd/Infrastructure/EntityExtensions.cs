using BackEnd.Data;
using ConferenceDTO.Response;
using System.Linq;

namespace BackEnd.Infrastructure
{
    public static class EntityExtensions
    {
        public static ConferenceResponse MapConferenceResponse(this Conference conference) =>
            new ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name,
                Slug = conference.Slug,
                StartDate = conference.StartDate,
                EndDate = conference.EndDate,
                Sessions = conference.Sessions?
                                     .Select(s => new ConferenceDTO.Session
                                     {
                                         ID = s.ID,
                                         Title = s.Title,
                                         Slug = s.Slug,
                                         Abstract = s.Abstract,
                                         StartTime = s.StartTime,
                                         EndTime = s.EndTime, 
                                         TrackId = s.TrackId
                                     })
                                    .OrderBy(s => s.StartTime)
                                    .ToList(),
                Speakers = conference.Sessions?
                                     .SelectMany(s => s.SessionSpeakers)
                                     .Select(ss => new ConferenceDTO.Speaker
                                     {
                                         ID = ss.SpeakerID,
                                         Name = ss.Speaker.Name,
                                         Slug = ss.Speaker.Slug
                                     })
                                       .OrderBy(s => s.Name)
                                       .ToList(),
                Tracks = conference.Tracks?
                                    .Select(t => new ConferenceDTO.Track
                                    {
                                        TrackID = t.TrackID,
                                        Name = t.Name,
                                        Slug = t.Slug
                                    })
                                    .OrderBy(s => s.Name)
                                    .ToList()
            };

        public static SessionResponse MapSessionResponse(this Session session) =>
            new SessionResponse
            {
                ID = session.ID,
                Title = session.Title,
                Slug = session.Slug,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Tags = session.SessionTags?
                              .Select(st => new ConferenceDTO.Tag
                              {
                                  ID = st.TagID,
                                  Name = st.Tag.Name,
                                  Slug = st.Tag.Slug
                              })
                               .ToList(),
                Speakers = session.SessionSpeakers?
                                  .Select(ss => new ConferenceDTO.Speaker
                                  {
                                      ID = ss.SpeakerID,
                                      Name = ss.Speaker.Name,
                                      Slug = ss.Speaker.Slug
                                  })
                                   .ToList(),
                TrackId = session.TrackId,
                Track = new ConferenceDTO.Track
                {
                    TrackID = session.TrackId ?? 0,
                    Name = session.Track?.Name,
                    Slug = session.Track?.Slug
                },
                ConferenceID = session.ConferenceID,
                Conference = session.Conference == null ? default(ConferenceDTO.Conference) :
                    new ConferenceDTO.Conference
                    {
                        ID = session.ConferenceID,
                        Name = session.Conference.Name,
                        Slug = session.Conference.Slug,
                        StartDate = session.Conference.StartDate,
                        EndDate = session.Conference.EndDate
                    },
                Abstract = session.Abstract
            };

        public static SpeakerResponse MapSpeakerResponse(this Speaker speaker) =>
            new SpeakerResponse
            {
                ID = speaker.ID,
                Name = speaker.Name,
                Slug = speaker.Slug,
                Bio = speaker.Bio,
                WebSite = speaker.WebSite,
                Sessions = speaker.SessionSpeakers?
                    .Select(ss =>
                        new ConferenceDTO.Session
                        {
                            ID = ss.SessionID,
                            Title = ss.Session.Title,
                            Slug = ss.Session.Slug
                        })
                    .ToList()
            };

        public static AttendeeResponse MapAttendeeResponse(this Attendee attendee) =>
            new AttendeeResponse
            {
                ID = attendee.ID,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                UserName = attendee.UserName,
                EmailAddress = attendee.EmailAddress,
                AvatarId = attendee.AvatarId,
                Avatar = attendee.Avatar,

                Sessions = attendee.SessionsAttendees?
                    .Select(sa =>
                        new ConferenceDTO.Session
                        {
                            ID = sa.SessionID,
                            Title = sa.Session.Title,
                            Slug = sa.Session.Slug,
                            StartTime = sa.Session.StartTime,
                            EndTime = sa.Session.EndTime
                        })
                    .ToList(),
                Conferences = attendee.ConferenceAttendees?
                    .Select(ca =>
                        new ConferenceDTO.Conference
                        {
                            ID = ca.ConferenceID,
                            Name = ca.Conference.Name,
                            Slug = ca.Conference.Slug
                        })
                    .ToList(),
            };

        public static TrackResponse MapTrackResponse(this Track track) =>
            new TrackResponse
            {
                TrackID = track.TrackID,
                Name = track.Name,
                Slug = track.Slug,
                ConferenceID = track.ConferenceID,
                Conference = track.Conference == null ? default(ConferenceDTO.Conference) :
                    new ConferenceDTO.Conference
                    {
                        ID = track.ConferenceID,
                        Name = track.Conference.Name,
                        Slug = track.Conference.Slug,
                        StartDate = track.Conference.StartDate,
                        EndDate = track.Conference.EndDate
                    },
                Sessions = track.Sessions?
                    .Select(s => new ConferenceDTO.Session
                    {
                        ID = s.ID,
                        Title = s.Title,
                        Slug = s.Slug,
                        Abstract = s.Abstract,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                    })
                .OrderBy(s => s.StartTime)
                .ToList()
            };
    }
}
