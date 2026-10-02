using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftBloom.Consumer;
using SoftBloom.Modelos;

namespace SoftBloom.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PedidosController : Controller
    {
        private static readonly string[] Estados = { "Pendiente", "En proceso", "Terminado", "Entregado" };

        // Lista de todos los pedidos
        public IActionResult Index()
        {
            var pedidos = CRUD<Pedido>.GetAll()
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            ViewBag.Clientes = CRUD<Cliente>.GetAll().ToList();
            ViewBag.Detalles = CRUD<DetallePedido>.GetAll().ToList();

            return View(pedidos);
        }

        // Muestra el pedido con su detalle y el formulario para cambiar el estado
        public IActionResult Gestionar(int? id)
        {
            if (id == null) return NotFound();

            var pedido = CRUD<Pedido>.GetById(id.Value);
            if (pedido == null) return NotFound();

            ViewBag.Cliente = CRUD<Cliente>.GetAll().FirstOrDefault(c => c.IdCliente == pedido.IdCliente);
            ViewBag.Detalles = CRUD<DetallePedido>.GetAll().Where(d => d.IdPedido == pedido.IdPedido).ToList();
            ViewBag.Productos = CRUD<Producto>.GetAll().ToList();
            ViewBag.Estados = Estados;

            return View(pedido);
        }

        // Guarda el nuevo estado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Gestionar(int id, string estado)
        {
            if (!Estados.Contains(estado))
            {
                TempData["Error"] = "Elige un estado válido.";
                return RedirectToAction(nameof(Gestionar), new { id });
            }

            var pedido = CRUD<Pedido>.GetById(id);
            if (pedido == null) return NotFound();

            try
            {
                pedido.EstadoPedido = estado;
                CRUD<Pedido>.Update(id, pedido);

                TempData["Mensaje"] = $"El pedido #{id} ahora está en estado: {estado}.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo actualizar el estado: " + ex.Message;
                return RedirectToAction(nameof(Gestionar), new { id });
            }
        }
    }
}