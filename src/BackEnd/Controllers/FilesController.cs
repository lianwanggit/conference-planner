using BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FilesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var file = await _db.Files.AsNoTracking()
                                      .SingleOrDefaultAsync(s => s.FileId == id);

            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }
    }
}
