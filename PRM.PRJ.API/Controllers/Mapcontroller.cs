using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM.PRJ.API.Data;
using PRM.PRJ.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRM.PRJ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Map
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Map>>> GetMaps()
        {
            return await _context.Map.ToListAsync();
        }

        // GET: api/Map/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Map>> GetMap(int id)
        {
            var map = await _context.Map.FindAsync(id);

            if (map == null)
            {
                return NotFound();
            }

            return map;
        }

        // POST: api/Map
        [HttpPost]
        public async Task<ActionResult<Map>> PostMap(Map map)
        {
            _context.Map.Add(map);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMap), new { id = map.Id }, map);
        }

        // PUT: api/Map/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMap(int id, Map map)
        {
            if (id != map.Id)
            {
                return BadRequest();
            }

            _context.Entry(map).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Map/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMap(int id)
        {
            var map = await _context.Map.FindAsync(id);
            if (map == null)
            {
                return NotFound();
            }

            _context.Map.Remove(map);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MapExists(int id)
        {
            return _context.Map.Any(e => e.Id == id);
        }
    }
}
