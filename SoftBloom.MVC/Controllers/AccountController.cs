using SoftBloom.Servicios.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace SoftBloom.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Account/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Account/Index
        [HttpPost]
        public async Task<IActionResult> Index(string correo, string contrasenia)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasenia))
            {
                ViewBag.Error = "Ingrese el correo y la contraseña.";
                return View();
            }

            var resultado = await _authService.Login(correo, contrasenia);

            if (!resultado)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(
            string nombre,
            string apellido,
            string correo,
            string nombreUsuario,
            string contrasenia,
            string confirmarContrasenia)
        {
            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(nombreUsuario) ||
                string.IsNullOrWhiteSpace(contrasenia))
            {
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View();
            }

            if (contrasenia != confirmarContrasenia)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            var resultado = await _authService.Register(
                nombre,
                apellido,
                correo,
                nombreUsuario,
                contrasenia
            );

            if (!resultado)
            {
                ViewBag.Error = "Ya existe un usuario con ese correo.";
                return View();
            }

            TempData["Mensaje"] = "Registro exitoso. Ahora puede iniciar sesión.";

            return RedirectToAction("Index");
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Account");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}