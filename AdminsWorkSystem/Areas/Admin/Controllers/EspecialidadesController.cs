using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Modelos;
using AdminsWorkSystem.Modelos.ViewModels;
using AdminsWorkSystem.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace AdminsWorkSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class EspecialidadesController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public EspecialidadesController(IUnidadTrabajo unidadTrabajo )
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            EspecialidadesVM especialidadesVM = new EspecialidadesVM()
            {
                Especialidades = new Especialidades(),
                UnidadLista = _unidadTrabajo.Unidades.ObtenerTodos().Select(u => new SelectListItem
                {
                    Text = u.Nombre,
                    Value = u.Id.ToString()
                }),
                               
            };

            if (id == null)
            {
                // Esto es para Crear nuevo Registro
                return View(especialidadesVM);
            }
            // Esto es para Actualizar
            especialidadesVM.Especialidades = _unidadTrabajo.Especialidades.Obtener(id.GetValueOrDefault());
            if (especialidadesVM.Especialidades == null)
            {
                return NotFound();
            }

            return View(especialidadesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(EspecialidadesVM especialidadesVM)
        {
            if (ModelState.IsValid)
            {
                            
                if (especialidadesVM.Especialidades.Id == 0)
                {
                    _unidadTrabajo.Especialidades.Agregar(especialidadesVM.Especialidades);
                }
                else
                {
                    _unidadTrabajo.Especialidades.Actualizar(especialidadesVM.Especialidades);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                especialidadesVM.UnidadLista= _unidadTrabajo.Unidades.ObtenerTodos().Select(u => new SelectListItem
                {
                    Text = u.Nombre,
                    Value = u.Id.ToString()
                });
               
                if (especialidadesVM.Especialidades.Id != 0)
                {
                    especialidadesVM.Especialidades = _unidadTrabajo.Especialidades.Obtener(especialidadesVM.Especialidades.Id);
                }

            }
            return View(especialidadesVM.Especialidades);
        }




        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Especialidades.ObtenerTodos(incluirPropiedades: "Unidades");
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var carreraDb = _unidadTrabajo.Especialidades.Obtener(id);
            if (carreraDb == null)
            {
                return Json(new { success = false, message = "Error al Borrar" });
            }            
            _unidadTrabajo.Especialidades.Remover(carreraDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Borrado Exitosamente" });
        }

        #endregion
    }
}

