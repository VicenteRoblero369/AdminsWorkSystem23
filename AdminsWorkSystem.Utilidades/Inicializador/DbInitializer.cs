using Microsoft.AspNetCore.Identity;
using AdminsWorkSystem.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminsWorkSystem.AccesoDatos;
using Microsoft.EntityFrameworkCore;
using AdminsWorkSystem.Utilidades;
using AdminsWorkSystem.Data;
using NPOI.SS.Formula.Functions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using AdminsWorkSystem.AccesoDatos.Repositorio;

namespace SistemaInvetario.Utilidades.Inicializador
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
       // private readonly UnidadTrabajo context;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }
            catch (Exception)
            {
                throw;
            }

            if (!_db.Unidades.Any())
            {
                var unidadesDatos = File.ReadAllText("../AdminsWorkSystem.AccesoDatos/SeedData/Unidades.json");
                var unidades = System.Text.Json.JsonSerializer.Deserialize<List<Unidades>>(unidadesDatos);

                foreach (var item in unidades)
                {
                    _db.Unidades.Add(item);
                }
                _db.SaveChanges();
            }
            if (!_db.Especialidades.Any())
            {
                var especialidadesDatos = File.ReadAllText("../AdminsWorkSystem.AccesoDatos/SeedData/Especialidades.json");
                var especialidades = System.Text.Json.JsonSerializer.Deserialize<List<Especialidades>>(especialidadesDatos);

                foreach (var item in especialidades)
                {
                    _db.Especialidades.Add(item);
                }
                _db.SaveChanges();
            }

            if (_db.Roles.Any(r => r.Name == DS.Role_Admin)) return;
            
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_ResponsableU)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_ResponsableC)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Coordinadores)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Estudiante)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_JefaDepartamento)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new UsuarioAplicacion
            {
                UserName = "ADMINROBLERO@GMAIL.COM",
                Email = "ADMINROBLERO@GMAIL.COM",
                EmailConfirmed = true,
                Nombres = "VICENTE",
                ApellidoPaterno = "ROBLERO",
                ApellidoMaterno = "ROBLERO",
                Sexo = "Hombre",
                PhoneNumber = "6681464117",
                UnidadesId = 1,
                EspecialidadesId = 1

            }, "Admin123*").GetAwaiter().GetResult();  // Password


            UsuarioAplicacion user = _db.UsuarioAplicacion.Where(u => u.UserName == "ADMINROBLERO@GMAIL.COM").FirstOrDefault();

            _userManager.AddToRoleAsync(user, DS.Role_Admin).GetAwaiter().GetResult();

        
        }
    }
}


