using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Data;
using AdminsWorkSystem.Modelos;
using AdminsWorkSystem.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;

namespace AdminsWorkSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;
        public UsuarioController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }   
        public IActionResult ObtenerLista()
        {
            return View();
        }
        #region API

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var usuarioLista = _unidadTrabajo.UsuarioAplicacion.ObtenerTodos(incluirPropiedades: "Unidades,Especialidades");
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new { data = usuarioLista });
        }
        [HttpGet]
        public IActionResult ObtenerOrdenLista()
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<UsuarioAplicacion> ordenlista;
            if (User.IsInRole(DS.Role_ResponsableU))
            {
                var userRole = _db.UserRoles.ToList();
                var roles = _db.Roles.ToList();
                var usuarioApp = _db.UsuarioAplicacion.Find(claim.Value);
                var Nombres = usuarioApp.UnidadesId;
                ordenlista = _unidadTrabajo.UsuarioAplicacion.ObtenerTodos(u => u.UnidadesId == Nombres, incluirPropiedades: "Especialidades.Unidades");
                foreach (var usuario in ordenlista)
                {
                    var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                    usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                }
                return Json(new { data = ordenlista });
            }
            if (User.IsInRole(DS.Role_ResponsableC))
            {
                var userRole = _db.UserRoles.ToList();
                var roles = _db.Roles.ToList();
                var usuarioApp = _db.UsuarioAplicacion.Find(claim.Value);//mandar a traer usuario
                var NombresU = usuarioApp.UnidadesId;
                var NombresC = usuarioApp.EspecialidadesId;
                ordenlista = _db.UsuarioAplicacion.Where(c => c.UnidadesId == NombresU).Where(c => c.EspecialidadesId == NombresC).Include(c => c.Unidades).Include(o => o.Especialidades);
                foreach (var usuario in ordenlista)
                {
                    var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                    usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                }
                return Json(new { data = ordenlista });
            }
            return RedirectToAction(nameof(Index));
            
        }
            [HttpPost]
        public IActionResult BloquearDesbloquear([FromBody] string id)
        {
            var usuario = _db.UsuarioAplicacion.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de Usuario" });
            }

            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                // Usuario Bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operacion Exitosa" });

        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var unidadDb = _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == id);
            if (unidadDb == null)
            {
                return Json(new { success = false, message = "Error al Borrar" });
            }
            _unidadTrabajo.UsuarioAplicacion.Remover(unidadDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Borrada Exitosamente" });
        }


        #endregion       
    }
}
