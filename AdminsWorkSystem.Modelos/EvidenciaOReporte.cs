using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdminsWorkSystem.Modelos
{
    public class EvidenciaOReporte
    {
        [Key]
        public int Id { get; set; }

        public string UsuarioAplicacionId { get; set; }

        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        [Required(ErrorMessage = "Este campo Fecha es Requerido")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "Este campo Fecha es Requerido")]
        public DateTime FechaFinal { get; set; }

        [Required(ErrorMessage = "Este campo {0} es Requerido")]
        [MaxLength(90)]
        [Display(Name = "Unidad Receptora")]
        public string UnidadReceptora { get; set; }

        [Required(ErrorMessage = "Este campo {0} es Requerido")]
        [MaxLength(100)]
        [Display(Name = "Programa")]
        public string Programa { get; set; }

        [Required(ErrorMessage = "Este campo {0} es Requerido")]
        [MaxLength(90)]
        [Display(Name = "Sub Programa")]
        public string SubPrograma { get; set; }

        [Required(ErrorMessage = "Este campo {0} es Requerido")]
        [MaxLength(90)]
        [Display(Name = "Actividad")]
        public string Actividad { get; set; }
        [Required]
        public DateTime FechaLiberacion { get; set; }
        public string Estado { get; set; }

        [Required(ErrorMessage = "Este campo Estado Requerido")]
        public string EstadoPais { get; set; }
        [Required(ErrorMessage = "Este campo Municipio es Requerido")]
        public string Municipio { get; set; }
        [Required(ErrorMessage = "Este campo es Requerido")]
        public string SemestreActual { get; set; }
        [Required(ErrorMessage = "Este campo es Requerido")]
        public string GrupoActual { get; set; }

        public string Archivo { get; set; }
        public string Imagenes { get; set; }
        public string statusUsTS { get; set; }

    }
}

