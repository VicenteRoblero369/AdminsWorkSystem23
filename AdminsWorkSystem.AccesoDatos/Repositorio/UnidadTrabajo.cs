using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IFormatosRepositorio Formatos { get; private set; }
        public IUsuarioAplicacionRepositorio UsuarioAplicacion { get; private set; }
        public IUnidadesRepositorio Unidades { get; private set; }
        public IGeneracionRepositorio Generacion { get; private set; }
        public IEspecialidadesRepositorio Especialidades { get; private set; }
        public IEvidenciaOReporteRepositorio EvidenciaOReporte { get; private set; }
        public IConstanciaRepositorio Constancia { get; private set; }
        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Formatos = new FormatosRepositorio(_db); // Inicializamos
            UsuarioAplicacion = new UsuarioAplicacionRepositorio(_db);
            Unidades = new UnidadesRepositorio(_db);
            Generacion = new GeneracionRepositorio(_db);
            Especialidades = new EspecialidadesRepositorio(_db);
            EvidenciaOReporte = new EvidenciaOReporteRepositorio(_db);
            Constancia = new ConstanciaRepositorio(_db);
        }

        public void Guardar()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
