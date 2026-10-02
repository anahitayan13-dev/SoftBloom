using Microsoft.AspNetCore.Mvc;
using SoftBloom.Consumer;
using SoftBloom.Modelos;
using Microsoft.AspNetCore.Authorization;


namespace SoftBloom.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View(CRUD<Cliente>.GetAll());
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = CRUD<Cliente>.GetById(id.Value);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            try
            {
                CRUD<Cliente>.Create(cliente);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(cliente);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = CRUD<Cliente>.GetById(id.Value);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
                return NotFound();

            try
            {
                CRUD<Cliente>.Update(id, cliente);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(cliente);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = CRUD<Cliente>.GetById(id.Value);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            CRUD<Cliente>.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}