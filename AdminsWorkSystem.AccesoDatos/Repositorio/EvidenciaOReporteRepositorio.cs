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
    public class EvidenciaOReporteRepositorio : Repositorio<EvidenciaOReporte>, IEvidenciaOReporteRepositorio
    {
        private readonly ApplicationDbContext _db;
               
        public EvidenciaOReporteRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(EvidenciaOReporte evidenciaOReporte)
        {
            var evidenciaDb = _db.evidenciaOReporte.FirstOrDefault(p => p.Id == evidenciaOReporte.Id);
            if (evidenciaDb != null)
            {
                if (evidenciaOReporte.Archivo != null)
                {
                    evidenciaDb.Archivo = evidenciaOReporte.Archivo;
                }
                //if (evidenciaOReporte.Imagenes != null)
                //{
                //    evidenciaDb.Imagenes = evidenciaOReporte.Imagenes;
                //}
                else
                {                 

                }
                evidenciaDb.UsuarioAplicacionId = evidenciaDb.UsuarioAplicacionId;
                evidenciaDb.FechaInicio = evidenciaOReporte.FechaInicio;
                evidenciaDb.FechaFinal = evidenciaOReporte.FechaFinal;
                evidenciaDb.UnidadReceptora = evidenciaOReporte.UnidadReceptora;
                evidenciaDb.Programa = evidenciaOReporte.Programa;
                evidenciaDb.SubPrograma = evidenciaOReporte.SubPrograma;
                evidenciaDb.Actividad = evidenciaOReporte.Actividad;
                evidenciaOReporte.FechaLiberacion = evidenciaOReporte.FechaLiberacion;
                evidenciaDb.Estado = evidenciaOReporte.Estado;
                evidenciaDb.Municipio = evidenciaOReporte.Municipio;
                evidenciaDb.SemestreActual = evidenciaOReporte.SemestreActual;
                evidenciaDb.GrupoActual = evidenciaOReporte.GrupoActual;
                evidenciaDb.statusUsTS = evidenciaOReporte.statusUsTS;
                evidenciaDb.EstadoPais = evidenciaOReporte.EstadoPais;
                evidenciaDb.Imagenes = evidenciaOReporte.Imagenes;

            }
        }
    }
}