using BackEnd.Data;
using BackEnd.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TracksController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TracksController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetTracks()
        {
            var tracks = await _db.Tracks.AsNoTracking()
                                         .Include(t => t.Conference)
                                         .Include(t => t.Sessions)
                                         .ToListAsync();

            var result = tracks.Select(t => t.MapTrackResponse());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTrack([FromRoute]int id)
        {
            var track = await _db.Tracks.AsNoTracking()
                                        .Include(t => t.Conference)
                                        .Include(t => t.Sessions)
                                        .SingleOrDefaultAsync(s => s.TrackID == id);
            if (track == null)
            {
                return NotFound();
            }
            var result = track.MapTrackResponse();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetTrackBySlug/{slug}")]
        public async Task<IActionResult> GetTrackBySlug([FromRoute]string slug)
        {
            var track = await _db.Tracks.AsNoTracking()
                                        .Include(t => t.Conference)
                                        .Include(t => t.Sessions)
                                        .FirstOrDefaultAsync(t => t.Slug == slug);
            if (track == null)
            {
                return NotFound();
            }
            var result = track.MapTrackResponse();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ConferenceDTO.Track input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var track = new Track
            {
                Name = input.Name,
                Slug = input.Name.ToSlug(),
                ConferenceID = input.ConferenceID
            };

            _db.Tracks.Add(track);
            await _db.SaveChangesAsync();

            var result = track.MapTrackResponse();

            return CreatedAtAction(nameof(GetTrack), new { id = track.TrackID }, result);
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTrack([FromRoute]int id, [FromBody]ConferenceDTO.Track input)
        {
            var track = await _db.FindAsync<Track>(id);

            if (track == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            track.Name = input.Name;
            track.Slug = input.Name.ToSlug();
            track.ConferenceID = input.ConferenceID;            

            // TODO: Handle exceptions, e.g. concurrency
            await _db.SaveChangesAsync();

            var result = track.MapTrackResponse();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var track = await _db.FindAsync<Track>(id);

            if (track == null)
            {
                return NotFound();
            }

            _db.Remove(track);

            // TODO: Handle exceptions, e.g. concurrency
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
