using AdminsWorkSystem.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadesRepositorio : IRepositorio<Unidades>
    {
        void Actualizar(Unidades unidades);
       
    }
}
