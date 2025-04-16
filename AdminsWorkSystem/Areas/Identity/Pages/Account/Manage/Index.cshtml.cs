using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Data;
using AdminsWorkSystem.Modelos;
using AdminsWorkSystem.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminsWorkSystem.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, 
            ApplicationDbContext db, IUnidadTrabajo unidadTrabajo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _unidadTrabajo = unidadTrabajo;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Nombres { get; set; }
            public string ApellidoMaterno { get; set; }
            public string ApelidoPaterno { get; set; }
            public string Generacion { get; set; }
         
        }
        

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //mas variables
            var nombres = "";
            var apellidoMaterno = "";
            var apellidoPaterno = "";
            var generacion = "";

            Username = userName;
            //traer el usuario que esta conectado
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            if (claim !=null)//que debuelve datos
            {
                var usuarioApp = await _db.UsuarioAplicacion.FindAsync(claim.Value);
                nombres = usuarioApp.Nombres;
                apellidoMaterno = usuarioApp.ApellidoMaterno;
                apellidoPaterno = usuarioApp.ApellidoPaterno;
                generacion = usuarioApp.Generacion;
            }

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Nombres = nombres,
                ApellidoMaterno=apellidoMaterno,
                ApelidoPaterno=apellidoPaterno,
                Generacion=generacion,

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //var nombres = await _userManager(user);
            //volver apreguntar el usuario conectado
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            var usuarioApp = new UsuarioAplicacion();
 
            if (claim != null)//si es diferente a nulo trae datos validos
            {
                //traer el usuario conectado
                usuarioApp = await _db.UsuarioAplicacion.FindAsync(claim.Value);
                usuarioApp.Nombres = Input.Nombres;
                usuarioApp.ApellidoMaterno = Input.ApellidoMaterno;
                usuarioApp.ApellidoPaterno = Input.ApelidoPaterno;
                usuarioApp.Generacion = Input.Generacion;

            }

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar configurar el número de teléfon";
                    return RedirectToPage();
                }
            }
            //if (Input.Nombres != nombres)
            //{
            //    var setNombreResult = await _userManager.SetPhoneNumberAsync(user, Input.Nombres);
            //    if (!setNombreResult.Succeeded)
            //    {
            //        StatusMessage = "Error inesperado al intentar configurar el Nombre";
            //        return RedirectToPage();
            //    }
            //}

            await _signInManager.RefreshSignInAsync(user);
            //actualizar los datos
            _db.UsuarioAplicacion.Update(usuarioApp);
            //grabar cambios
            await _db.SaveChangesAsync();
            StatusMessage = "Tu perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}
