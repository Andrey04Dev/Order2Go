using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Helpers;
using Proyecto_Order2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradorController : Controller
    {
        CodeStackCTX ctx;
        public AdministradorController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AgregarOperador()
        {
            ViewBag.Roles = await ctx.Roles.Where(x => x.IdRole == 2).ToListAsync();
            return View();
        }
        //La vista de un comercio.
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AgregarComercio()
        {
            Comercio comercio = new Comercio();
            //POner donde sean operador
            ViewBag.Usuario = await ctx.Usuario.Where(x => x.IdRole ==2).ToListAsync();
            return View(comercio);
        }
        //Funciones para agregar operardor y comercio
        [BindProperty]

        public Usuario usuario { get; set; }
        public Roles role { get; set; }

        [HttpPost]
        public async Task<IActionResult> AgregarOperadores()
        {
            //Verificamos si el correo si ha registrado.
            var result = await ctx.Usuario.Where(x => x.Correo == usuario.Correo).SingleOrDefaultAsync();
            if (result != null)
            {
                //Usamos LinQ del badrequest para mostrar el error.
                return BadRequest(new JObject()
                {
                    {"StatusCode", 404 },
                    {"Message","Este usuario ya existe. Ingrese uno nuevo." }
                });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    /*Al ModelState le aplicamos el selectmany para encotrar todos los errores 
                     que tengamos la clase Usuario. 
                    Ahí selecionamos los valores de los errores que va mostrar y lo muestra en 
                    una lista.*/
                    return BadRequest(ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
                }
                else
                {
                    var hash = HashHelper.Hash(usuario.Contraseña);
                    usuario.Contraseña = hash.Password;
                    usuario.Salt = hash.Salt;
                    //usuario.Roles.Add(new UsuarioRol {IdUsuario = result.IdUsuario ,IdRole = 2 });
                    ctx.Usuario.Add(usuario);
                    await ctx.SaveChangesAsync();
                    usuario.Contraseña = "";
                    usuario.Salt = "";
                    return Ok(usuario);
                }
            }
        }
        //Agregamos un comercio.
        [BindProperty]
        public Comercio comercio { get; set; }

        [HttpPost]
        public async Task<IActionResult> AgregarComercios()
        {
            //Verificamos si el correo si ha registrado.
            var result = await ctx.Comercio.Where(x => x.Nombre == comercio.Nombre).SingleOrDefaultAsync();
            if (result != null)
            {
                //Usamos LinQ del badrequest para mostrar el error.
                return BadRequest(new JObject()
                {
                    {"StatusCode", 404 },
                    {"Message","Ingrese un nombre de compañia que no exista." }
                });
            }
            else
            {
                ctx.Comercio.Add(comercio);
                await ctx.SaveChangesAsync();
                return Ok(comercio);
            }
        }
    }
}
