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
    public class AttendeesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AttendeesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var attendees = await _db.Attendees.Include(a => a.Avatar)
                                               .Include(a => a.SessionsAttendees)
                                                    .ThenInclude(sa => sa.Session)
                                               .ToListAsync();
            var result = attendees.Select(s => s.MapAttendeeResponse()).ToList();

            return Ok(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var attendee = await _db.Attendees.Include(a => a.Avatar)
                                              .Include(a => a.SessionsAttendees)
                                                .ThenInclude(sa => sa.Session)
                                              .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
            {
                return NotFound();
            }

            var result = attendee.MapAttendeeResponse();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ConferenceDTO.Attendee input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var avatar = input.Avatar;
            if (avatar != null)
            {                
                _db.Files.Add(avatar);
                await _db.SaveChangesAsync();

                input.AvatarId = avatar.FileId;
            }

            var attendee = new Attendee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName,
                EmailAddress = input.EmailAddress,
                AvatarId = input.AvatarId
            };

            _db.Attendees.Add(attendee);
            await _db.SaveChangesAsync();

            var result = attendee.MapAttendeeResponse();

            return CreatedAtAction(nameof(Get), new { username = result.UserName }, result);
        }

        [HttpPost("{username}/session/{sessionId:int}")]
        public async Task<IActionResult> AddSession(string username, int sessionId)
        {
            var attendee = await _db.Attendees.Include(a => a.SessionsAttendees)
                                                .ThenInclude(sa => sa.Session)
                                              .Include(a => a.ConferenceAttendees)
                                                .ThenInclude(ca => ca.Conference)
                                              .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
            {
                return NotFound();
            }

            var session = await _db.Sessions.FindAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            attendee.SessionsAttendees.Add(new SessionAttendee
            {
                AttendeeID = attendee.ID,
                SessionID = sessionId
            });

            await _db.SaveChangesAsync();

            var result = attendee.MapAttendeeResponse();

            return Ok(result);
        }

        [HttpDelete("{username}/session/{sessionId:int}")]
        public async Task<IActionResult> RemoveSession(string username, int sessionId)
        {
            var attendee = await _db.Attendees.Include(a => a.SessionsAttendees)
                                              .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
            {
                return NotFound();
            }

            var session = await _db.Sessions.FindAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            var sessionAttendee = attendee.SessionsAttendees.FirstOrDefault(sa => sa.SessionID == sessionId);
            attendee.SessionsAttendees.Remove(sessionAttendee);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}