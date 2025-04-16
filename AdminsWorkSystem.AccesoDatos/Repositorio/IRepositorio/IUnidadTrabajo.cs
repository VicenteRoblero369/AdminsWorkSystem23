using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IFormatosRepositorio Formatos { get; }
        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }
        IUnidadesRepositorio Unidades { get; }
        IGeneracionRepositorio Generacion { get; }
        IEspecialidadesRepositorio Especialidades { get; }
        IEvidenciaOReporteRepositorio EvidenciaOReporte { get; }
        IConstanciaRepositorio Constancia { get; }

        void Guardar();
    }
}
