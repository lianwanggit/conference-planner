using BackEnd.Data;
using BackEnd.Infrastructure;
using ConferenceDTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ConferencesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ConferencesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetConferences()
        {
            var conferences = await _db.Conferences.AsNoTracking()
                                                   .Include(c => c.Tracks)
                                                   .Include(c => c.Sessions)
                                                        .ThenInclude(ct => ct.SessionTags)
                                                            .ThenInclude(ctt => ctt.Tag)
                                                   .Include(c => c.Sessions)
                                                        .ThenInclude(cs => cs.SessionSpeakers)
                                                            .ThenInclude(css => css.Speaker)
                                                   .ToListAsync();
            var result = conferences.Select(s => s.MapConferenceResponse()).ToList();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetConference([FromRoute] int id)
        {
            var conference = await _db.Conferences.AsNoTracking()
                                            .Include(c => c.Tracks)
                                            .Include(c => c.Sessions)             
                                                .ThenInclude(ct => ct.SessionTags)
                                                    .ThenInclude(ctt => ctt.Tag)
                                            .Include(c => c.Sessions)
                                                .ThenInclude(cs => cs.SessionSpeakers)
                                                    .ThenInclude(css=> css.Speaker)
                                            .SingleOrDefaultAsync(c => c.ID == id);

            if (conference == null)
            {
                return NotFound();
            }

            var result = conference.MapConferenceResponse();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetConferenceBySlug/{slug}")]
        public async Task<IActionResult> GetConferenceBySlug([FromRoute] string slug)
        {
            var conference = await _db.Conferences.AsNoTracking()
                                            .Include(c => c.Tracks)
                                            .Include(c => c.Sessions)
                                                .ThenInclude(ct => ct.SessionTags)
                                                    .ThenInclude(ctt => ctt.Tag)
                                            .Include(c => c.Sessions)
                                                .ThenInclude(cs => cs.SessionSpeakers)
                                                    .ThenInclude(css => css.Speaker)
                                            .SingleOrDefaultAsync(c => c.Slug == slug);

            if (conference == null)
            {
                return NotFound();
            }

            var result = conference.MapConferenceResponse();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConference([FromBody] ConferenceDTO.Conference input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conference = new Conference
            {
                Name = input.Name,
                Slug = input.Name.ToSlug(),
                StartDate = input.StartDate,
                EndDate = input.EndDate                
            };

            _db.Conferences.Add(conference);
            await _db.SaveChangesAsync();

            var result = conference.MapConferenceResponse();

            return CreatedAtAction(nameof(GetConference), new { id = conference.ID }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateConference([FromRoute]int id, [FromBody]ConferenceDTO.Conference input)
        {
            var conference = await _db.FindAsync<Conference>(id);

            if (conference == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            conference.Name = input.Name;
            conference.Slug = input.Name.ToSlug();
            conference.StartDate = input.StartDate;
            conference.EndDate = input.EndDate;

            await _db.SaveChangesAsync();

            var result = conference.MapConferenceResponse();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteConference([FromRoute] int id)
        {
            var conference = await _db.FindAsync<Conference>(id);

            if (conference == null)
            {
                return NotFound();
            }

            _db.Remove(conference);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}