using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Data;
using AdminsWorkSystem.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdminsWorkSystem.AccesoDatos.Repositorio
{
    public class UnidadesRepositorio : Repositorio<Unidades>, IUnidadesRepositorio
    {
        private readonly ApplicationDbContext _db;

        public UnidadesRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Unidades unidades)
        {
            var unidadDb = _db.Unidades.FirstOrDefault(u => u.Id == unidades.Id);
            if (unidadDb != null)
            {
                unidadDb.Nombre = unidades.Nombre;
                //marcaDb.Estado = marca.Estado;
            }
        }
    }
}


