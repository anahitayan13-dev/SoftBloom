using System.Security.Claims;
using SoftBloom.Consumer;
using SoftBloom.Modelos;
using SoftBloom.Servicios.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SoftBloom.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(string correo, string contrasenia)
        {
            var usuarios = CRUD<Usuario>.GetAll();

            // Buscar ignorando mayúsculas, minúsculas y espacios innecesarios
            var usuario = usuarios.FirstOrDefault(u =>
                u.correo != null && u.correo.Trim().Equals(correo.Trim(), StringComparison.OrdinalIgnoreCase));

            if (usuario == null)
            {
                return false;
            }

            bool passwordCorrecta = false;

            // Soporte híbrido: verifica con BCrypt o con texto plano si es un usuario antiguo
            try
            {
                passwordCorrecta = BCrypt.Net.BCrypt.Verify(contrasenia, usuario.contrasenia);
            }
            catch
            {
                passwordCorrecta = (usuario.contrasenia == contrasenia);
            }

            if (!passwordCorrecta)
            {
                return false;
            }

            var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, usuario.nombre ?? string.Empty),
    new Claim(ClaimTypes.Email, usuario.correo ?? string.Empty),
    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
    new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(usuario.rol) ? "Cliente" : usuario.rol)
};

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!.SignInAsync("Cookies", principal);

            return true;
        }

        public async Task<bool> Register(
            string nombre,
            string apellido,
            string correo,
            string nombreUsuario,
            string contrasenia)
        {
            var usuarios = CRUD<Usuario>.GetAll();

            var existe = usuarios.Any(u =>
                u.correo != null && u.correo.Trim().Equals(correo.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existe)
            {
                return false;
            }

            // La contraseña se envía en texto plano: la API (UsuariosController)
            // es la única que aplica el hash con BCrypt.
            var usuario = new Usuario
            {
                nombre = nombre,
                apellido = apellido,
                correo = correo.Trim(),
                nombreUsuario = nombreUsuario,
                contrasenia = contrasenia
            };

            CRUD<Usuario>.Create(usuario);

            return await Task.FromResult(true);
        }
    }
}