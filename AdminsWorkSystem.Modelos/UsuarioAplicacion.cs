using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace AdminsWorkSystem.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {

        [Required]
        public string Nombres { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? NumeroEmpleado { get; set; }
        public string? Sexo { get; set; }
        public string? Matricula { get; set; }
        public string? LenguaMaterna { get; set; }
        public string? Generacion { get; set; }  
        public bool Estatus { get; set; }
        public string? Status { get; set; } 
        [NotMapped]
        public string Role { get; set; }

        //forinkey
        public int UnidadesId { get; set; }

        [ForeignKey("UnidadesId")]
        public Unidades Unidades { get; set; }

        public int EspecialidadesId { get; set; }

        [ForeignKey("EspecialidadesId")]
        public Especialidades Especialidades { get; set; }

    }
}
