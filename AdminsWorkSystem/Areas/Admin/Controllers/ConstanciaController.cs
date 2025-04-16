using AdminsWorkSystem.AccesoDatos.Repositorio;
using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Data;
using AdminsWorkSystem.Modelos.ViewModels;
using AdminsWorkSystem.Modelos;
using AdminsWorkSystem.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AdminsWorkSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ConstanciaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public ConstanciaVM ConstanciaViewModel { get; set; }

        public ConstanciaController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment hostEnvironment, ApplicationDbContext db, UserManager<IdentityUser> userManage)
        {
            _unidadTrabajo = unidadTrabajo;
            _hostEnvironment = hostEnvironment;
            _db = db;
            _userManager = userManage;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_JefaDepartamento + "," + DS.Role_ResponsableU + "," + DS.Role_ResponsableC + "," + DS.Role_Coordinadores)]
        public IActionResult Solicitando(int id)
        {
            Constancia constancia = _unidadTrabajo.Constancia.ObtenerPrimero(o => o.Id == id);
            constancia.Estatus = DS.EstadoEnviados;
            _unidadTrabajo.Guardar();
            return RedirectToAction("Upsert");
        }
        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_JefaDepartamento + "," + DS.Role_ResponsableU + "," + DS.Role_ResponsableC + "," + DS.Role_Coordinadores)]
        public IActionResult Entregados(int id)
        {
            Constancia constancia = _unidadTrabajo.Constancia.ObtenerPrimero(o => o.Id == id);
            constancia.Estatus = DS.EstadoEntregado;
            _unidadTrabajo.Guardar();
            return RedirectToAction("Upsert");
        }
        public IActionResult Upsert(int? id)
        {

            Constancia constancia = new Constancia();
      
            if (id == null)
            {
                // Esto es para Crear nuevo Registro
                return View(constancia);
            }
            //Esto es para Actualizar
            constancia = _unidadTrabajo.Constancia.Obtener(id.GetValueOrDefault());
            if (constancia == null)
            {
                return NotFound();
            }
            return View(constancia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public IActionResult Upsert(Constancia constancia)
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
           // constaciaVM.Constancia.UsuarioAplicacion = _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(e => e.Id == claim.Value);
            constancia.UsuarioAplicacionId = claim.Value;
            //constaciaVM.Constancia.Estatus = DS.EstadoEntregado;

            if (ModelState.IsValid)
            {
                // Cargar Imagenes
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"imagenes\Constancias");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (constancia.Archivo != null)
                    {
                        //Esto es para editar, necesitamos borrar la imagen anterior
                        var imagenPath = Path.Combine(webRootPath, constancia.Archivo.TrimStart('\\'));
                        if (System.IO.File.Exists(imagenPath))
                        {
                            System.IO.File.Delete(imagenPath);
                        }
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    constancia.Archivo = @"\imagenes\Constancias\" + filename + extension;
                }
                else
                {
                    // Si en el Update el usuario no cambia la imagen
                    if (constancia.Id != 0)
                    {
                        Constancia constanciaDB = _unidadTrabajo.Constancia.Obtener(constancia.Id);
                        constancia.Archivo = constanciaDB.Archivo;
                    }
                }
                if (constancia.Id == 0)
                {
                    _unidadTrabajo.Constancia.Agregar(constancia);
                }
                else
                {
                    _unidadTrabajo.Constancia.Actualizar(constancia);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            else
            {

                if (constancia.Id != 0)
                {
                    constancia = _unidadTrabajo.Constancia.Obtener(constancia.Id);
                }

            }
            return View(constancia);
        }
        #region
        [HttpGet]
        public IActionResult ObtenerListaConstancia(string estado, int id)
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Constancia> ordenlista;
            if (User.IsInRole(DS.Role_Admin) || User.IsInRole(DS.Role_JefaDepartamento))
            {
                ordenlista = _unidadTrabajo.Constancia.ObtenerTodos(incluirPropiedades: "UsuarioAplicacion.Especialidades.Unidades");
            }
            else if (User.IsInRole(DS.Role_ResponsableU))
            {
                var usuarioApp = _db.UsuarioAplicacion.Find(claim.Value);
                var Nombres = usuarioApp.UnidadesId;
                ordenlista = _unidadTrabajo.Constancia.ObtenerTodos(u => u.UsuarioAplicacion.UnidadesId == Nombres, incluirPropiedades: "UsuarioAplicacion.Especialidades.Unidades");

            }
            else if (User.IsInRole(DS.Role_ResponsableC))
            {
                var usuarioApp = _db.UsuarioAplicacion.Find(claim.Value);
                var NombresU = usuarioApp.UnidadesId;
                var NombresC = usuarioApp.EspecialidadesId;
                ordenlista = _db.Constancia.Where(c => c.UsuarioAplicacion.UnidadesId == NombresU).Where(c => c.UsuarioAplicacion.EspecialidadesId == NombresC).Include(c => c.UsuarioAplicacion.Unidades).Include(o => o.UsuarioAplicacion.Especialidades);
            }
            else if (User.IsInRole(DS.Role_Estudiante))
            {
                var usuarioApp = _db.UsuarioAplicacion.Find(claim.Value);
                var NombresU = usuarioApp.UnidadesId;
                var NombresC = usuarioApp.EspecialidadesId;
                var MatriculaUs = usuarioApp.Matricula;
                ordenlista = _db.Constancia.Where(c => c.UsuarioAplicacion.UnidadesId == NombresU).Where(c => c.UsuarioAplicacion.EspecialidadesId == NombresC).Where(c => c.MyProperty == MatriculaUs).Include(c => c.UsuarioAplicacion.Unidades).Include(o => o.UsuarioAplicacion.Especialidades);
            }
            else
            {
                ordenlista = _db.Constancia.Where(o => o.UsuarioAplicacionId == claim.Value).Include(o => o.UsuarioAplicacion.Unidades).Include(o => o.UsuarioAplicacion.Especialidades);
                //ordenlista = _unidadTrabajo.EvidenciaOReporte.ObtenerTodos(o => o.UsuarioAplicacionId == claim.Value, incluirPropiedades: "UsuarioAplicacion.Unidades");
            }
            switch (estado)
            {
                case "entregado":
                    ordenlista = ordenlista.Where(o => o.Estatus == DS.EstadoEntregado);
                    break;
                case "solicitando":
                    ordenlista = ordenlista.Where(o => o.Estatus == DS.EstadoEnviados);
                    break;           
                default:
                    break;

            }

            return Json(new { data = ordenlista });
        }
        #endregion
    }
}
