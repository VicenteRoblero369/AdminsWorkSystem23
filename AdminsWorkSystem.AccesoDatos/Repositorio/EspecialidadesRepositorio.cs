using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Data;
using AdminsWorkSystem.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.AccesoDatos.Repositorio
{
    public class EspecialidadesRepositorio : Repositorio<Especialidades>, IEspecialidadesRepositorio
    {
        private readonly ApplicationDbContext _db;

        public EspecialidadesRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Especialidades especialidades)
        {
            var carreraDb = _db.Especialidades.FirstOrDefault(p => p.Id == especialidades.Id);
            if (carreraDb != null)//si carrera exite en la base de datos
            {                           
                carreraDb.Nombre = especialidades.Nombre;
                carreraDb.UnidadesId = especialidades.UnidadesId;

            }
        }
    }
}
