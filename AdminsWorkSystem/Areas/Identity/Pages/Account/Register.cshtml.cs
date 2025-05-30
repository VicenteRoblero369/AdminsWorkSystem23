﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AdminsWorkSystem.AccesoDatos.Repositorio.IRepositorio;
using AdminsWorkSystem.Modelos;
using AdminsWorkSystem.Utilidades;
using EllipticCurve;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AdminsWorkSystem.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, RoleManager<IdentityRole> roleManager,
            IUnidadTrabajo unidadTrabajo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unidadTrabajo = unidadTrabajo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public string UserName { get; set; }
           // [Required(ErrorMessage ="Campo Email Es Requerido")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }
            public string PhoneNumber { get; set; }
            [Required(ErrorMessage = "Este campo {0} es Requerido")]
            [MaxLength(90)]
            [Display(Name = "Nombres")]
            public string Nombres { get; set; }
            [Required(ErrorMessage = "Este campo {0} es Requerido")]
            [MaxLength(90)]
            [Display(Name = "ApellidoM")]
            public string ApellidoPaterno { get; set; }
            [Required(ErrorMessage = "Este campo {0} es Requerido")]
            [MaxLength(90)]
            [Display(Name = "ApellidoP")]
            public string ApellidoMaterno { get; set; }
            //[Required(ErrorMessage = "Este campo {0} es Requerido")]
            //[MaxLength(20)]
            //[Display(Name = "Matricula")]
            public string Matricula { get; set; }
            public string NumeroEmpleado { get; set; }

            [Required(ErrorMessage = "Este campo {0} es Requerido")]
            [MaxLength(90)]
            [Display(Name = "Género")]
            public string Sexo { get; set; }
            public string LenguaMaterna { get; set; }         
            public string Generacion { get; set; }
            [Required]
            public bool Estatus { get; set; }         
            public int Unidades { get; set; }
            public int Especialidades { get; set; }         
            public string Status { get; set; }         
            public string Role { get; set; }           

            public string[] Genders = new[] { "Hombre", "Mujer" };
            public IEnumerable<SelectListItem> ListaRol { get; set; }
            public IEnumerable<SelectListItem> ListaRolv { get; set; }
            public IEnumerable<SelectListItem> ListaGeneracion { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;          
            
            Input = new InputModel()
            {
                ListaRol = _roleManager.Roles.Where(r => r.Name != DS.Role_Estudiante).Where(r => r.Name != DS.Role_Admin).Select(n => n.Name).Select(l => new SelectListItem
                {
                    Text = l,
                    Value = l
                }),
                ListaRolv = _roleManager.Roles.Where(r => r.Name != DS.Role_Estudiante).Where(r => r.Name != DS.Role_Admin).Where(r => r.Name != DS.Role_JefaDepartamento).Select(n => n.Name).Select(l => new SelectListItem
                {
                    Text = l,
                    Value = l
                }),
                ListaGeneracion = _unidadTrabajo.Generacion.ObtenerTodos().Select(g => new SelectListItem
                {
                    Text = g.Nombre,
                    //Value = g.Id.ToString()
                }),
                
            };

                      
            
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new UsuarioAplicacion
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nombres = Input.Nombres,
                    ApellidoPaterno = Input.ApellidoPaterno,
                    ApellidoMaterno = Input.ApellidoMaterno,
                    NumeroEmpleado = Input.NumeroEmpleado,
                    Sexo = Input.Sexo,
                    LenguaMaterna = Input.LenguaMaterna,
                    Generacion = Input.Generacion,
                    Matricula = Input.Matricula,
                    Estatus = Input.Estatus,                  
                    UnidadesId = Input.Unidades,
                    EspecialidadesId = Input.Especialidades,
                    Role = Input.Role,
                    PhoneNumber = Input.PhoneNumber,
                    Status = Input.Status
                };

                //var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {                   
                        _logger.LogInformation("User created a new account with password.");

                    if (!await _roleManager.RoleExistsAsync(DS.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Estudiante))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Estudiante));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_ResponsableU))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_ResponsableU));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_ResponsableC))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_ResponsableC));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Coordinadores))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Coordinadores));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_JefaDepartamento))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_JefaDepartamento));
                    }
                    //si usuario registra y en pantalla no seleciona rol entonces se adigna como usuario
                    if (user.Role == null)
                        {
                            await _userManager.AddToRoleAsync(user, DS.Role_Estudiante);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, user.Role);
                    }
                    //var userId = await _userManager.GetUserIdAsync(user);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Role == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            // Administrador esta registrando un nuevo usuario
                            return RedirectToAction("Index", "Usuario", new { Area = "Admin" });
                        }

                    }
                }

                Input = new InputModel()
                {
                    ListaRol = _roleManager.Roles.Where(r => r.Name != DS.Role_Estudiante).Where(r => r.Name != DS.Role_Admin).Select(n => n.Name).Select(l => new SelectListItem
                    {
                        Text = l,
                        Value = l
                    }),
                    ListaRolv = _roleManager.Roles.Where(r => r.Name != DS.Role_Estudiante).Where(r => r.Name != DS.Role_Admin).Where(r => r.Name != DS.Role_JefaDepartamento).Select(n => n.Name).Select(l => new SelectListItem
                    {
                        Text = l,
                        Value = l
                    }),

                };

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
