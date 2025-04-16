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
    public class UnidadesController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public UnidadesController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Unidades unidades = new Unidades();
            if (id == null)
            {
                // Esto es para Crear nuevo Registro
                return View(unidades);
            }
            // Esto es para Actualizar
            unidades = _unidadTrabajo.Unidades.Obtener(id.GetValueOrDefault());
            if (unidades == null)
            {
                return NotFound();
            }

            return View(unidades);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Unidades unidades)
        {
            if (ModelState.IsValid)
            {
                if (unidades.Id == 0)
                {
                    _unidadTrabajo.Unidades.Agregar(unidades);
                }
                else
                {
                    _unidadTrabajo.Unidades.Actualizar(unidades);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(unidades);
        }



        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Unidades.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var unidadDb = _unidadTrabajo.Unidades.Obtener(id);
            if (unidadDb == null)
            {
                return Json(new { success = false, message = "Error al Borrar" });
            }
            _unidadTrabajo.Unidades.Remover(unidadDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Borrada Exitosamente" });
        }

        #endregion
    }
}

