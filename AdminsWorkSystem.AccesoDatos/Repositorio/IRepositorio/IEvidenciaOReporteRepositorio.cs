using AdminsWorkSystem.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio
{
    public interface IEvidenciaOReporteRepositorio : IRepositorio<EvidenciaOReporte>
    {
        void Actualizar(EvidenciaOReporte evidenciaOReporte);
    }
}
