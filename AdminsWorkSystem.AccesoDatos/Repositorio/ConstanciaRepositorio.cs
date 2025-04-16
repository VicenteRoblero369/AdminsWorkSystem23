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
    public class ConstanciaRepositorio : Repositorio<Constancia>, IConstanciaRepositorio
    {
        private readonly ApplicationDbContext _db;
               
        public ConstanciaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Constancia constancia)
        {
            var constanciaDb = _db.Constancia.FirstOrDefault(p => p.Id == constancia.Id);
            if (constanciaDb != null)
            {
                if (constancia.Archivo != null)
                {
                    constancia.Archivo = constancia.Archivo;
                }
                else
                {                 

                }
                constancia.UsuarioAplicacionId = constancia.UsuarioAplicacionId;
                constancia.Estatus = constancia.Estatus;
                constancia.MyProperty = constancia.MyProperty;
            }
        }
    }
}