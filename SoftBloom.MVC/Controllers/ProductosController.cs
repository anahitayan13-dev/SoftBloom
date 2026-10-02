using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftBloom.Consumer;
using SoftBloom.Modelos;

namespace SoftBloom.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductosController : Controller
    {
        // Carga las listas para los desplegables y para mostrar nombres
        private void CargarListas()
        {
            ViewBag.Categorias = CRUD<Categoria>.GetAll().ToList();
            ViewBag.Materiales = CRUD<Material>.GetAll().ToList();
        }

        public IActionResult Index()
        {
            CargarListas();
            return View(CRUD<Producto>.GetAll());
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var producto = CRUD<Producto>.GetById(id.Value);
            if (producto == null) return NotFound();
            return View(producto);
        }

        public IActionResult Create()
        {
            CargarListas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            try
            {
                CRUD<Producto>.Create(producto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarListas();
                return View(producto);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var producto = CRUD<Producto>.GetById(id.Value);
            if (producto == null) return NotFound();
            CargarListas();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Producto producto)
        {
            if (id != producto.IdProducto) return NotFound();
            try
            {
                CRUD<Producto>.Update(id, producto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarListas();
                return View(producto);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var producto = CRUD<Producto>.GetById(id.Value);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            CRUD<Producto>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}