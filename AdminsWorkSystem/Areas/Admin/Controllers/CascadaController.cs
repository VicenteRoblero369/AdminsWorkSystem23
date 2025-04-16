using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AdminsWorkSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CascadaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnidadTrabajo _unidadTrabajo;
        public CascadaController(ApplicationDbContext db, IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }
        public IActionResult Index()
        {
            //var pais = _db.Pais.OrderBy(x => x.Nombres).ToList();
            return View();
        }


        public JsonResult ObtenerTodos()
        {
            var todos = _db.Unidades.OrderBy(x => x.Nombre).ToList();
            return new JsonResult(todos);
        }
        public JsonResult ObtenerTodosEspecialidades(int id)
        {
            var estados = _db.Especialidades.Where(x => x.Unidades.Id == id).OrderBy(x => x.Nombre).ToList();
            return new JsonResult(estados);
            // return Json(new { data = todos });
        }

    }
}
