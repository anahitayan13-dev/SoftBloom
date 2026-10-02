using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SoftBloom.Modelos;
using SoftBloom.Consumer;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace SoftBloom.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DetallesPedidosController : Controller
    {
        public IActionResult Index()
        {
            var detalles = CRUD<DetallePedido>.GetAll();

            var productos = CRUD<Producto>.GetAll();
            var pedidos = CRUD<Pedido>.GetAll();
            var clientes = CRUD<Cliente>.GetAll();

            var nombresProductos = productos.ToDictionary(
                p => p.IdProducto,
                p => p.NombreProducto
            );

            var nombresClientes = clientes.ToDictionary(
                c => c.IdCliente,
                c => $"{c.Nombres} {c.Apellidos}"
            );

            var nombresPedidos = pedidos.ToDictionary(
                p => p.IdPedido,
                p =>
                {
                    if (nombresClientes.TryGetValue(p.IdCliente, out var cliente))
                    {
                        return $"Pedido de {cliente}";
                    }

                    return $"Pedido";
                }
            );

            ViewBag.NombresProductos = nombresProductos;
            ViewBag.NombresPedidos = nombresPedidos;

            return View(detalles);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var detalle = CRUD<DetallePedido>.GetById(id.Value);
            if (detalle == null) return NotFound();
            return View(detalle);
        }

        private void CargarListasDesplegables()
        {
            ViewBag.Pedidos = CRUD<Pedido>.GetAll()
                .Select(p => new SelectListItem
                {
                    Value = p.IdPedido.ToString(),
                    Text = $"Pedido #{p.IdPedido} - {p.FechaPedido:dd/MM/yyyy}"
                })
                .ToList();

            ViewBag.Productos = CRUD<Producto>.GetAll()
                .Select(pr => new SelectListItem { Value = pr.IdProducto.ToString(), Text = pr.NombreProducto })
                .ToList();
        }

        public IActionResult Create()
        {
            CargarListasDesplegables();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DetallePedido detalle)
        {
            try
            {
                CRUD<DetallePedido>.Create(detalle);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarListasDesplegables();
                return View(detalle);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var detalle = CRUD<DetallePedido>.GetById(id.Value);
            if (detalle == null) return NotFound();
            CargarListasDesplegables();
            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DetallePedido detalle)
        {
            if (id != detalle.IdDetalle) return NotFound();
            try
            {
                CRUD<DetallePedido>.Update(id, detalle);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarListasDesplegables();
                return View(detalle);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var detalle = CRUD<DetallePedido>.GetById(id.Value);
            if (detalle == null) return NotFound();
            return View(detalle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            CRUD<DetallePedido>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
