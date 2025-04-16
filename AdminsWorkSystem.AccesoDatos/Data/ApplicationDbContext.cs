using AdminsWorkSystem.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminsWorkSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       public DbSet<Formatos> Formatos { get; set; }    
       public DbSet<Generacion> Generacion { get; set; }
       public DbSet<Especialidades> Especialidades { get; set; }
       public DbSet<Unidades>Unidades { get; set; }
       public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }
       public DbSet<EvidenciaOReporte> evidenciaOReporte { get; set; }
       //public DbSet<FormatosEnvios> FormatosEnvios { get; set; }
       public DbSet<Constancia> Constancia { get; set; }
      
    }
}
