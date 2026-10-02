using SoftBloom.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SoftBloom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly SoftBloomAPIContext _context;

        public MaterialesController(SoftBloomAPIContext context)
        {
            _context = context;
        }

        // GET: api/Materiales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMateriales()
        {
            return await _context.Materiales.ToListAsync();
        }

        // GET: api/Materiales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await _context.Materiales.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            return material;
        }

        // POST: api/Materiales
        [HttpPost]
        public async Task<ActionResult<Material>> PostMaterial(Material material)
        {
            _context.Materiales.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterial), new { id = material.IdMaterial }, material);
        }

        // PUT: api/Materiales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(int id, Material material)
        {
            if (id != material.IdMaterial)
            {
                return BadRequest();
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // DELETE: api/Materiales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materiales.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materiales.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists(int id)
        {
            return _context.Materiales.Any(e => e.IdMaterial == id);
        }
    }
}