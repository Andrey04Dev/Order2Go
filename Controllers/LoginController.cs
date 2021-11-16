using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Helpers;
using Proyecto_Order2Go.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    public class LoginController : Controller
    {
        CodeStackCTX ctx;

        public LoginController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        public IActionResult Index()
        {
            return View();
        }
        [BindProperty]
        public UsuarioVM Usuario { get; set; }
        public async Task<IActionResult> Login()
        {
            if (!ModelState.IsValid)
            {
                return NotFound(new JObject()
                    {
                        {"StatusCode", 404 },
                        {"Message","El usuario no ha sido encontrado." }
                    });
            }
            else
            {
                var result = await ctx.Usuario.Include("Roles.Rol").Where(x => x.Correo == Usuario.Correo).SingleOrDefaultAsync();
                if (result == null)
                {
                    return NotFound(new JObject()
                    {
                        {"StatusCode", 404 },
                        {"Message","El usuario no ha sido encontrado." }
                    });
                }
                else
                {
                    if (HashHelper.CheckHash(Usuario.Contraseña, result.Contraseña, result.Salt))
                    {
                        //Validamos si tiene rol
                        if (result.Roles.Count == 0)
                        {
                            return NotFound(new JObject()
                            {
                                {"StatusCode", 404 },
                                {"Message","No tiene acceso al sistema." }
                            });
                        }
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.IdUsuario.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Name, result.Nombre));
                        identity.AddClaim(new Claim(ClaimTypes.Email, result.Correo));
                        identity.AddClaim(new Claim("Dato", "Valor"));

                        foreach (var Rol in result.Roles)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, Rol.Rol.Descripción));
                        }

                        var principal = new ClaimsPrincipal(identity);
                        /*Especificaciones que se hace la autentificacion por cookie, El dia de expiración es de 1 día,
                         si no ha pasado el tiempo de expiración, se loguea solo.*/
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                            new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddHours(1), IsPersistent = true });
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }
    }
}