﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.Modelos.ViewModels
{
    public class EspecialidadesVM
    {
        public Especialidades Especialidades { get; set; }
        public IEnumerable<SelectListItem> UnidadLista { get; set; }
    }
}
