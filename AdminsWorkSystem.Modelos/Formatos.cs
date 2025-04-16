using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.Modelos
{
    public class Formatos
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre Es Requerido")]
        [MaxLength(100, ErrorMessage = "40 caracteres maximo")]
        public string Nombre { get; set; } 

        //[Required(ErrorMessage = "Es Requerido")]
        public string FormatosUrl { get; set; }
    }
}
