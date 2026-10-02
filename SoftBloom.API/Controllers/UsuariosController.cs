using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftBloom.Modelos;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly SoftBloomAPIContext _context;

    public UsuariosController(SoftBloomAPIContext context)
    {
        _context = context;
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    // GET: api/Usuarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return usuario;
    }

    // PUT: api/Usuarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest();
        }

        var usuarioExistente = await _context.Usuarios.FindAsync(id);

        if (usuarioExistente == null)
        {
            return NotFound();
        }

        usuarioExistente.nombre = usuario.nombre;
        usuarioExistente.apellido = usuario.apellido;
        usuarioExistente.correo = usuario.correo;

        // Solo cambia la contraseña si el usuario escribió una nueva
        if (!string.IsNullOrWhiteSpace(usuario.contrasenia))
        {
            usuarioExistente.contrasenia =
                BCrypt.Net.BCrypt.HashPassword(usuario.contrasenia);
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Usuarios
    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        // Verificar si ya existe un correo

        var existe = await _context.Usuarios
            .AnyAsync(u => u.correo.ToLower() == usuario.correo.ToLower());

        if (existe)
        {
            return Conflict("Ya existe un usuario con el mismo correo.");
        }

        // Encriptar la contraseña antes de guardarla
        usuario.contrasenia =
            BCrypt.Net.BCrypt.HashPassword(usuario.contrasenia);
        usuario.rol = "Cliente"; // el registro público siempre crea clientes
        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUsuario),
            new { id = usuario.Id },
            usuario
        );
    }

    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}