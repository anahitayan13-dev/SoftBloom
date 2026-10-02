using Microsoft.AspNetCore.Mvc;
using SoftBloom.Consumer;
using SoftBloom.Modelos;
using Microsoft.AspNetCore.Authorization;


namespace SoftBloom.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MaterialesController : Controller
    {
        public IActionResult Index()
        {
            return View(CRUD<Material>.GetAll());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var material = CRUD<Material>.GetById(id.Value);

            if (material == null)
                return NotFound();

            return View(material);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material material)
        {
            try
            {
                CRUD<Material>.Create(material);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(material);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var material = CRUD<Material>.GetById(id.Value);

            if (material == null)
                return NotFound();

            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Material material)
        {
            if (id != material.IdMaterial)
                return NotFound();

            try
            {
                CRUD<Material>.Update(id, material);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(material);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var material = CRUD<Material>.GetById(id.Value);

            if (material == null)
                return NotFound();

            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            CRUD<Material>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}