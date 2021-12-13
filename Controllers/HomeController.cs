using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Helpers;
using Proyecto_Order2Go.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CodeStackCTX ctx;
        public HomeController(ILogger<HomeController> logger, CodeStackCTX _ctx)
        {
            _logger = logger;
            ctx = _ctx;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Comercio = await ctx.Comercio.ToListAsync();
            Comercio comercio = new Comercio();
            return View(comercio);
        }

        //AllowAnonymous permite que cualquier persona ingrese a esa página
        [AllowAnonymous]
        public IActionResult Registrar()
        {
            return View();
        }
        [BindProperty]
        public Usuario Usuario { get; set; }
        public UsuarioRol UsuarioRol { get; set; }
        public Roles Rol { get; set; }
        [HttpPost]
        public async Task<IActionResult> Registro()
        {
            //Verificamos si el correo si ha registrado.
            var result = await ctx.Usuario.Where(x => x.Correo == Usuario.Correo).SingleOrDefaultAsync();
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
                if (!ModelState.IsValid)
                {
                    /*Al ModelState le aplicamos el selectmany para encotrar todos los errores 
                     que tengamos la clase Usuario. 
                    Ahí selecionamos los valores de los errores que va mostrar y lo muestra en 
                    una lista.*/
                    return BadRequest(ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList()); ;
                }
                else
                {
                    var hash = HashHelper.Hash(Usuario.Contraseña);
                    Usuario.Contraseña = hash.Password;
                    Usuario.Salt = hash.Salt;
                    Usuario.IdRole = 3;
                    ctx.Usuario.Add(Usuario);
                    await ctx.SaveChangesAsync();
                    Usuario.Contraseña = "";
                    Usuario.Salt = "";
                    return Ok(Usuario);
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
