using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminsWorkSystem.Modelos
{
    public class Especialidades
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string Nombre { get; set; }

        public int UnidadesId { get; set; }

        [ForeignKey("UnidadesId")]
        public Unidades Unidades { get; set; }
        
    }
}
