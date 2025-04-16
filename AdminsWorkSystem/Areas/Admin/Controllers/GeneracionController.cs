using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Modelos;
using AdminsWorkSystem.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AdminsWorkSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class GeneracionController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public GeneracionController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Generacion generacion = new Generacion();
            if (id == null)
            {
                // Esto es para Crear nuevo Registro
                return View(generacion);
            }
            // Esto es para Actualizar
            generacion = _unidadTrabajo.Generacion.Obtener(id.GetValueOrDefault());
            if (generacion == null)
            {
                return NotFound();
            }

            return View(generacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Generacion generacion)
        {
            if (ModelState.IsValid)
            {
                if (generacion.Id == 0)
                {
                    _unidadTrabajo.Generacion.Agregar(generacion);
                }
                else
                {
                    _unidadTrabajo.Generacion.Actualizar(generacion);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(generacion);
        }



        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Generacion.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var marcaDb = _unidadTrabajo.Generacion.Obtener(id);
            if (marcaDb == null)
            {
                return Json(new { success = false, message = "Error al Borrar" });
            }
            _unidadTrabajo.Generacion.Remover(marcaDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Borrada Exitosamente" });
        }

        #endregion
    }
}

