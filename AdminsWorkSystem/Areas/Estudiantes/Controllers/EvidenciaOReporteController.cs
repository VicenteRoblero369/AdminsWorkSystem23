using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdminsWorkSystem.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using AdminsWorkSystem.Modelos;
using AdminsWorkSystem.Modelos.ViewModels;
using NPOI.SS.Formula.Functions;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using AdminsWorkSystem.Data;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace AdminsWorkSystem.Areas.Estudiantes.Controllers
{
    [Area("Estudiantes")]
    [Authorize]
    public class EvidenciaOReporteController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _db;

        [BindProperty]  
        public EvidenciaOReporteVM EvidenciaOReporteViewModel { get; set; }

        public EvidenciaOReporteController(IUnidadTrabajo unidadTrabajo, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment,ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _unidadTrabajo = unidadTrabajo;
            _emailSender = emailSender;
            _userManager = userManager;
            _hostEnvironment = webHostEnvironment;
            _db= db;    
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Validacion()
        {
            return View();
        }                     
        public IActionResult Detalle(int id)
        {
            EvidenciaOReporteViewModel = new EvidenciaOReporteVM()
            {
                EvidenciaOReporte = _unidadTrabajo.EvidenciaOReporte.ObtenerPrimero(e => e.Id == id, incluirPropiedades: "UsuarioAplicacion"),              
            };
            return View(EvidenciaOReporteViewModel);

        }
        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_JefaDepartamento + "," + DS.Role_ResponsableU + "," + DS.Role_ResponsableC)]
        public IActionResult Aprovado(int id)
        {
            EvidenciaOReporte evidenciaOReporte = _unidadTrabajo.EvidenciaOReporte.ObtenerPrimero(o => o.Id == id);
            evidenciaOReporte.Estado = DS.EstadoAprobado;
            _unidadTrabajo.Guardar();
            return RedirectToAction("Validacion");
        }
        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_JefaDepartamento + "," + DS.Role_ResponsableU + "," + DS.Role_ResponsableC)]
        public IActionResult Cancelado(int id)
        {
            EvidenciaOReporte evidenciaOReporte = _unidadTrabajo.EvidenciaOReporte.ObtenerPrimero(o => o.Id == id);
            evidenciaOReporte.Estado = DS.EstadoCancelado;
            _unidadTrabajo.Guardar();
            return RedirectToAction("Validacion");
        }
        //------------------------------------------------------------------------------------
        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_JefaDepartamento + "," + DS.Role_ResponsableU + "," + DS.Role_ResponsableC)]
        public IActionResult Finalizado(string id)
        {
            UsuarioAplicacion usuarioAplicacion = _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(o => o.Id == id);
            usuarioAplicacion.Status = DS.EstadoTerminado;
            _unidadTrabajo.Guardar();
            return RedirectToAction("Validacion");
        }
        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_JefaDepartamento + "," + DS.Role_ResponsableU + "," + DS.Role_ResponsableC)]
        public IActionResult Baja(string id)
        {
            UsuarioAplicacion usuarioAplicacion = _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(o => o.Id == id);
            usuarioAplicacion.Status = DS.EstadoBaja;
            _unidadTrabajo.Guardar();
            return RedirectToAction("Validacion");
        }
        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_JefaDepartamento + "," + DS.Role_ResponsableU + "," + DS.Role_ResponsableC)]
        public IActionResult Reingreso(string id)
        {
            UsuarioAplicacion usuarioAplicacion = _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(o => o.Id == id);
            usuarioAplicacion.Status = DS.EstadoRengreso;
            _unidadTrabajo.Guardar();
            return RedirectToAction("Validacion");
        }
        public IActionResult Upsert(int? id)
        {
            EvidenciaOReporteVM evidenciaOReporteVM = new EvidenciaOReporteVM()
            {
            EvidenciaOReporte = new EvidenciaOReporte(),               
            };

            if (id == null)
            {
                // Esto es para Crear nuevo Registro
                return View(evidenciaOReporteVM);
            }
            //Esto es para Actualizar
            evidenciaOReporteVM.EvidenciaOReporte = _unidadTrabajo.EvidenciaOReporte.Obtener(id.GetValueOrDefault());
            if (evidenciaOReporteVM.EvidenciaOReporte == null)
            {
                return NotFound();
            }

            return View(evidenciaOReporteVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(EvidenciaOReporteVM evidenciaOReporteVM)
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            evidenciaOReporteVM.EvidenciaOReporte.UsuarioAplicacion = _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(e => e.Id == claim.Value);
            evidenciaOReporteVM.EvidenciaOReporte.UsuarioAplicacionId = claim.Value;
            evidenciaOReporteVM.EvidenciaOReporte.Estado = DS.EstadoEnviado;

            if (ModelState.IsValid)
            {
                // Cargar Imagenes
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"imagenes\ValidacionFile");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (evidenciaOReporteVM.EvidenciaOReporte.Archivo != null)
                    {
                        //Esto es para editar, necesitamos borrar la imagen anterior
                        var imagenPath = Path.Combine(webRootPath, evidenciaOReporteVM.EvidenciaOReporte.Archivo.TrimStart('\\'));
                        if (System.IO.File.Exists(imagenPath))
                        {
                            System.IO.File.Delete(imagenPath);
                        }
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    evidenciaOReporteVM.EvidenciaOReporte.Archivo = @"\imagenes\ValidacionFile\" + filename + extension;
                }
                else
                {
                    // Si en el Update el usuario no cambia la imagen
                    if (evidenciaOReporteVM.EvidenciaOReporte.Id != 0)
                    {
                        EvidenciaOReporte reporteDB = _unidadTrabajo.EvidenciaOReporte.Obtener(evidenciaOReporteVM.EvidenciaOReporte.Id);
                        evidenciaOReporteVM.EvidenciaOReporte.Archivo = reporteDB.Archivo;
                    }
                }
                if (evidenciaOReporteVM.EvidenciaOReporte.Id == 0)
                {
                    _unidadTrabajo.EvidenciaOReporte.Agregar(evidenciaOReporteVM.EvidenciaOReporte);
                }
                else
                {
                    _unidadTrabajo.EvidenciaOReporte.Actualizar(evidenciaOReporteVM.EvidenciaOReporte);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                
                if (evidenciaOReporteVM.EvidenciaOReporte.Id != 0)
                {
                    evidenciaOReporteVM.EvidenciaOReporte = _unidadTrabajo.EvidenciaOReporte.Obtener(evidenciaOReporteVM.EvidenciaOReporte.Id);
                }

            }
            return View(evidenciaOReporteVM.EvidenciaOReporte);
        }

        #region
        [HttpGet]
        public IActionResult ObtenerOrdenLista(string estado, int id)
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<EvidenciaOReporte> ordenlista;          
            if (User.IsInRole(DS.Role_Admin) || User.IsInRole(DS.Role_JefaDepartamento))
            {               
                ordenlista = _unidadTrabajo.EvidenciaOReporte.ObtenerTodos(incluirPropiedades: "UsuarioAplicacion.Especialidades.Unidades");
            }
            else if (User.IsInRole(DS.Role_ResponsableU))
            {               
                var usuarioApp = _db.UsuarioAplicacion.Find(claim.Value);
                var Nombres = usuarioApp.UnidadesId;
                ordenlista = _unidadTrabajo.EvidenciaOReporte.ObtenerTodos(u => u.UsuarioAplicacion.UnidadesId == Nombres, incluirPropiedades: "UsuarioAplicacion.Especialidades.Unidades");
                
            }
            else if (User.IsInRole(DS.Role_ResponsableC)) 
            {
                var usuarioApp = _db.UsuarioAplicacion.Find(claim.Value);
                var NombresU = usuarioApp.UnidadesId;
                var NombresC = usuarioApp.EspecialidadesId;
                ordenlista = _db.evidenciaOReporte.Where(c => c.UsuarioAplicacion.UnidadesId == NombresU).Where(c => c.UsuarioAplicacion.EspecialidadesId == NombresC).Include(c => c.UsuarioAplicacion.Unidades).Include(o => o.UsuarioAplicacion.Especialidades);
            }
            else
            {
                ordenlista = _db.evidenciaOReporte.Where(o => o.UsuarioAplicacionId == claim.Value).Include(o => o.UsuarioAplicacion.Unidades).Include(o => o.UsuarioAplicacion.Especialidades);
               //ordenlista = _unidadTrabajo.EvidenciaOReporte.ObtenerTodos(o => o.UsuarioAplicacionId == claim.Value, incluirPropiedades: "UsuarioAplicacion.Unidades");
            }
            switch (estado)
            {
                case "pendiente":
                    ordenlista = ordenlista.Where(o => o.Estado == DS.EstadoPendiente);
                    break;
                case "aprobado":
                    ordenlista = ordenlista.Where(o => o.Estado == DS.EstadoAprobado);
                    break;
                case "enviado":
                    ordenlista = ordenlista.Where(o => o.Estado == DS.EstadoEnviado);
                    break;
                case "cancelado":
                    ordenlista = ordenlista.Where(o => o.Estado == DS.EstadoCancelado);
                    break;
                case "activo":
                    ordenlista = ordenlista.Where(o => o.UsuarioAplicacion.Status == DS.EstadoActivo);
                    break;
                case "baja":
                    ordenlista = ordenlista.Where(o => o.UsuarioAplicacion.Status == DS.EstadoBaja);
                    break;
                case "reingreso":
                    ordenlista = ordenlista.Where(o => o.UsuarioAplicacion.Status == DS.EstadoRengreso);
                    break;
                case "finalizado":
                    ordenlista = ordenlista.Where(o => o.UsuarioAplicacion.Status == DS.EstadoTerminado);
                    break;
                default:
                    break;

            }

            return Json(new { data = ordenlista });
        }
        #endregion

        //public IActionResult ImprimirOrden(int id, EvidenciaOReporteVM evidenciaOReporteVM)
        //{
        //    evidenciaOReporteVM.EvidenciaOReporte = _unidadTrabajo.EvidenciaOReporte.ObtenerPrimero(o => o.Id == id, incluirPropiedades: "UsuarioAplicacion.Especialidades.Unidades");
        //    return new ViewAsPdf("ImprimirOrden", evidenciaOReporteVM)
        //    {
        //        FileName = "Evidencia#" + evidenciaOReporteVM.EvidenciaOReporte.Id + ".pdf",
        //        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
        //        PageSize = Rotativa.AspNetCore.Options.Size.A4,
        //        CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
        //    };

        //}

    }
}
