using BackEnd.Data;
using ConferenceDTO;
using ConferenceDTO.Response;
using ConferenceDTO.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody]SearchTerm term)
        {
            var query = term.Query;
            var sessionResults = await _db.Sessions.Include(s => s.Track)
                                                   .Include(s => s.SessionSpeakers)
                                                     .ThenInclude(ss => ss.Speaker)
                                                   .Where(s =>
                                                       s.Title.Contains(query) ||
                                                       s.Track.Name.Contains(query)
                                                   )
                                                   .ToListAsync();

            var speakerResults = await _db.Speakers.Include(s => s.SessionSpeakers)
                                                     .ThenInclude(ss => ss.Session)
                                                   .Where(s =>
                                                       s.Name.Contains(query) ||
                                                       s.Bio.Contains(query) ||
                                                       s.WebSite.Contains(query)
                                                   )
                                                   .ToListAsync();

            var results = sessionResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Session,
                Value = JObject.FromObject(new SessionResponse
                {
                    ID = s.ID,
                    Title = s.Title,
                    Slug = s.Slug,
                    Abstract = s.Abstract,
                    ConferenceID = s.ConferenceID,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    TrackId = s.TrackId,
                    Track = new ConferenceDTO.Track
                    {
                        TrackID = s?.TrackId ?? 0,
                        Name = s.Track?.Name
                    },
                    Speakers = s?.SessionSpeakers
                                 .Select(ss => new ConferenceDTO.Speaker
                                 {
                                     ID = ss.SpeakerID,
                                     Name = ss.Speaker.Name,
                                     Slug = ss.Speaker.Slug
                                 })
                                 .ToList()
                })
            })
            .Concat(speakerResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Speaker,
                Value = JObject.FromObject(new SpeakerResponse
                {
                    ID = s.ID,
                    Name = s.Name,
                    Slug = s.Slug,
                    Bio = s.Bio,
                    WebSite = s.WebSite,
                    Sessions = s.SessionSpeakers?
                                .Select(ss =>
                                    new ConferenceDTO.Session
                                    {
                                        ID = ss.SessionID,
                                        Title = ss.Session.Title,
                                        Slug = ss.Session.Slug
                                    })
                                .ToList()
                })
            }));

            return Ok(results);
        }
    }
}