using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftBloom.Consumer;
using SoftBloom.Modelos;

namespace SoftBloom.MVC.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class PedidoClienteController : Controller
    {
        // Busca el Cliente que corresponde al usuario logueado (por correo)
        private Cliente? ObtenerClienteActual()
        {
            var correo = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(correo)) return null;

            return CRUD<Cliente>.GetAll().FirstOrDefault(c =>
                c.Email != null &&
                c.Email.Trim().Equals(correo.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // Muestra el formulario
        public IActionResult Nuevo(int? idCategoria, int? idMaterial)
        {
            var productos = new List<Producto>();

            // Los productos solo se cargan cuando ya eligió categoría y material
            if (idCategoria.HasValue && idMaterial.HasValue)
            {
                productos = CRUD<Producto>.GetAll()
                    .Where(p => p.Stock > 0
                             && p.IdCategoria == idCategoria.Value
                             && p.IdMaterial == idMaterial.Value)
                    .ToList();
            }

            ViewBag.Categorias = CRUD<Categoria>.GetAll().ToList();
            ViewBag.Materiales = CRUD<Material>.GetAll().ToList();
            ViewBag.Productos = productos;
            ViewBag.IdCategoria = idCategoria;
            ViewBag.IdMaterial = idMaterial;
            ViewBag.ClienteExiste = ObtenerClienteActual() != null;

            return View();
        }

        // Guarda el pedido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(int idProducto, int cantidad, string? observaciones,
                                   string? cedula, string? telefono,
                                   int? idCategoria, int? idMaterial)
        {
            // Para volver al formulario sin perder la categoría y el material
            var volver = new { idCategoria, idMaterial };

            

            if (idProducto <= 0)
            {
                TempData["Error"] = "Elige un producto.";
                return RedirectToAction(nameof(Nuevo), volver);
            }

            var producto = CRUD<Producto>.GetById(idProducto);
            if (producto == null)
            {
                TempData["Error"] = "El producto no existe.";
                return RedirectToAction(nameof(Nuevo), volver);
            }

            if (cantidad < 1)
            {
                TempData["Error"] = "La cantidad debe ser al menos 1.";
                return RedirectToAction(nameof(Nuevo), volver);
            }

            if (cantidad > producto.Stock)
            {
                TempData["Error"] = $"Solo hay {producto.Stock} unidades disponibles.";
                return RedirectToAction(nameof(Nuevo), volver);
            }

            var cliente = ObtenerClienteActual();

            if (cliente == null && (string.IsNullOrWhiteSpace(cedula) || cedula.Trim().Length != 10))
            {
                TempData["Error"] = "Ingresa una cédula válida de 10 dígitos.";
                return RedirectToAction(nameof(Nuevo), volver);
            }

            try
            {
                // 1. Si es su primer pedido, crear el cliente
                if (cliente == null)
                {
                    var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    var usuario = CRUD<Usuario>.GetById(idUsuario);

                    CRUD<Cliente>.Create(new Cliente
                    {
                        Cedula = cedula!.Trim(),
                        Nombres = usuario?.nombre ?? "Cliente",
                        Apellidos = usuario?.apellido ?? "Cliente",
                        Telefono = telefono,
                        Email = usuario?.correo
                    });

                    cliente = ObtenerClienteActual();
                }

                // 2. Crear el pedido (la descripción va en Observaciones)
                CRUD<Pedido>.Create(new Pedido
                {
                    FechaPedido = DateTime.Now,
                    EstadoPedido = "Pendiente",
                    IdCliente = cliente!.IdCliente
                });

                // 3. Buscar el pedido recién creado (el último de este cliente)
                var pedido = CRUD<Pedido>.GetAll()
                    .Where(p => p.IdCliente == cliente.IdCliente)
                    .OrderByDescending(p => p.IdPedido)
                    .First();

                // 4. Crear el detalle del pedido
                CRUD<DetallePedido>.Create(new DetallePedido
                {
                    IdPedido = pedido.IdPedido,
                    IdProducto = producto.IdProducto,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio
                });

                // 5. Descontar el stock
                producto.Stock -= cantidad;
                CRUD<Producto>.Update(producto.IdProducto, producto);

                TempData["Mensaje"] = "¡Tu pedido se realizó con éxito!";
                return RedirectToAction(nameof(MisPedidos));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo crear el pedido: " + ex.Message;
                return RedirectToAction(nameof(Nuevo), volver);
            }
        }

        // Pedidos del cliente logueado
        public IActionResult MisPedidos()
        {
            var pedidos = new List<Pedido>();
            var cliente = ObtenerClienteActual();

            if (cliente != null)
            {
                pedidos = CRUD<Pedido>.GetAll()
                    .Where(p => p.IdCliente == cliente.IdCliente)
                    .OrderByDescending(p => p.FechaPedido)
                    .ToList();
            }

            ViewBag.Detalles = CRUD<DetallePedido>.GetAll().ToList();
            ViewBag.Productos = CRUD<Producto>.GetAll().ToList();

            return View(pedidos);
        }
    }
}