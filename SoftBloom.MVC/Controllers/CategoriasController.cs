using Microsoft.AspNetCore.Mvc;
using SoftBloom.Consumer;
using SoftBloom.Modelos;
using Microsoft.AspNetCore.Authorization;

namespace SoftBloom.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View(CRUD<Categoria>.GetAll());
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var categoria = CRUD<Categoria>.GetById(id.Value);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            try
            {
                CRUD<Categoria>.Create(categoria);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoria);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var categoria = CRUD<Categoria>.GetById(id.Value);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria categoria)
        {
            if (id != categoria.IdCategoria) return NotFound();
            try
            {
                CRUD<Categoria>.Update(id, categoria);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoria);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var categoria = CRUD<Categoria>.GetById(id.Value);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            CRUD<Categoria>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
