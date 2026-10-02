using SoftBloom.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SoftBloom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesPedidosController : ControllerBase
    {
        private readonly SoftBloomAPIContext _context;

        public DetallesPedidosController(SoftBloomAPIContext context)
        {
            _context = context;
        }

        // GET: api/DetallesPedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedido()
        {
            return await _context.DetallesPedidos.ToListAsync();
        }

        // GET: api/DetallesPedido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>> GetDetallePedido(int id)
        {
            var detalle = await _context.DetallesPedidos.FindAsync(id);

            if (detalle == null)
            {
                return NotFound();
            }

            return detalle;
        }

        // POST: api/DetallesPedido
        [HttpPost]
        public async Task<ActionResult<DetallePedido>> PostDetallePedido(DetallePedido detalle)
        {
            _context.DetallesPedidos.Add(detalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetallePedido), new { id = detalle.IdDetalle }, detalle);
        }

        // PUT: api/DetallesPedido/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePedido(int id, DetallePedido detalle)
        {
            if (id != detalle.IdDetalle)
            {
                return BadRequest();
            }

            _context.Entry(detalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePedidoExists(id))
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

        // DELETE: api/DetallesPedido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePedido(int id)
        {
            var detalle = await _context.DetallesPedidos.FindAsync(id);
            if (detalle == null)
            {
                return NotFound();
            }

            _context.DetallesPedidos.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallePedidoExists(int id)
        {
            return _context.DetallesPedidos.Any(e => e.IdDetalle == id);
        }
    }
}