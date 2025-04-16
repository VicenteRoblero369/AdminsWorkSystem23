using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.Modelos
{
    public class Constancia
    {
        [Key]
        public int Id { get; set; }
        public string Estatus { get; set; }
        public string Archivo { get; set; }
        public string MyProperty { get; set; }

        public string UsuarioAplicacionId { get; set; }
        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
    }
}
