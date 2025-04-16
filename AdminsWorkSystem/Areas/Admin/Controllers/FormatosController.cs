using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using AdminsWorkSystem.Modelos.ViewModels;
using AdminsWorkSystem.Utilidades;

namespace AdminsWorkSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class FormatosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FormatosController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {

            Formatos formatos = new Formatos();
                      
            if (id == null)
            {
                // Esto es para Crear nuevo Registro
                return View(formatos);
            }
            // Esto es para Actualizar
            formatos = _unidadTrabajo.Formatos.Obtener(id.GetValueOrDefault());
            if (formatos == null)
            {
                return NotFound();
            }

            return View(formatos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Formatos formatos)
        {
            if (ModelState.IsValid)
            {

                // Cargar Imagenes
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"imagenes\formatos");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (formatos.FormatosUrl != null)
                    {
                        //Esto es para editar, necesitamos borrar la imagen anterior
                        var imagenPath = Path.Combine(webRootPath, formatos.FormatosUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagenPath))
                        {
                            System.IO.File.Delete(imagenPath);
                        }
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    formatos.FormatosUrl = @"\imagenes\formatos\" + filename + extension;
                }
                else
                {
                    // Si en el Update el usuario no cambia la imagen
                    if (formatos.Id != 0)
                    {
                        Formatos formatoDB = _unidadTrabajo.Formatos.Obtener(formatos.Id);
                        formatos.FormatosUrl = formatoDB.FormatosUrl;
                    }
                }


                if (formatos.Id == 0)
                {
                    _unidadTrabajo.Formatos.Agregar(formatos);
                }
                else
                {
                    _unidadTrabajo.Formatos.Actualizar(formatos);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                

                if (formatos.Id != 0)
                {
                    formatos = _unidadTrabajo.Formatos.Obtener(formatos.Id);
                }

            }
            return View(formatos);
        }

        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Formatos.ObtenerTodos();
            return Json(new { data = todos });
        }
        [Authorize(Roles = DS.Role_Admin)]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var formatoDb = _unidadTrabajo.Formatos.Obtener(id);
            if (formatoDb == null)
            {
                return Json(new { success = false, message = "Error al Borrar" });
            }
            // Eliminar la Imagen relacionada al formato
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagenPath = Path.Combine(webRootPath, formatoDb.FormatosUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagenPath))
            {
                System.IO.File.Delete(imagenPath);
            }

            _unidadTrabajo.Formatos.Remover(formatoDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Archivo Borrado Exitosamente" });
        }

        #endregion
    }
}
