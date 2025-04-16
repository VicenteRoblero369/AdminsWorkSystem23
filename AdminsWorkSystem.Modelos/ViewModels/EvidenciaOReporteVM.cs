using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.Modelos.ViewModels
{
    public class EvidenciaOReporteVM
    {
        public EvidenciaOReporte EvidenciaOReporte { get; set; }
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
        public Unidades Unidades { get; set; }
        public IEnumerable<EvidenciaOReporte> EvidenciaLista { get; set; }
    }
}
