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
    public class GeneracionRepositorio : Repositorio<Generacion>, IGeneracionRepositorio
    {
        private readonly ApplicationDbContext _db;

        public GeneracionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Generacion generacion)
        {
            var GeneracionDb = _db.Generacion.FirstOrDefault(g => g.Id == generacion.Id);
            if (GeneracionDb != null)
            {
                GeneracionDb.Nombre = generacion.Nombre;
                //marcaDb.Estado = marca.Estado;
            }
        }
    }
}



