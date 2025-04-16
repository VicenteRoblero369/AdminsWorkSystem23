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
    public class FormatosRepositorio : Repositorio<Formatos>, IFormatosRepositorio
    {

        private readonly ApplicationDbContext _db;

        public FormatosRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Formatos formatos)
        {
            var foamtosBD = _db.Formatos.FirstOrDefault(b => b.Id == formatos.Id);
            if (foamtosBD != null)
            {
                if (formatos.FormatosUrl != null)
                {
                    foamtosBD.FormatosUrl = formatos.FormatosUrl;
                }
                foamtosBD.Nombre = formatos.Nombre;

                _db.SaveChanges();
            }
        }

    }
}

