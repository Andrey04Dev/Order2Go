using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Helpers;
using Proyecto_Order2Go.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    [Authorize]
    public class ModulosController : Controller
    {
        CodeStackCTX ctx;
        public ModulosController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        /*public IActionResult Index()
        {
            return View();
        }*/
        [Authorize(Roles = "Administrador")]

        public IActionResult AgregarOperador()
        {
            return View();
        }
        //La vista de un comercio.
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AgregarComercio()
        {
            Comercio comercio = new Comercio();
            //POner donde sean operador
            var result = await ctx.Usuario.Select(x => new { ID = x.IdUsuario, Nombre = x.Nombre, Apellidos = x.Apellidos, IdRole = x.Roles.FirstOrDefault(x => x.IdRol == 2) }).Where(x => x.IdRole.IdRol == 2).ToListAsync();
            ViewBag.Usuario = result.Select(x => new Usuario { IdUsuario = x.ID, Nombre = x.Nombre, Apellidos = x.Apellidos }).ToList();
            return View();
        }
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
                if (!ModelState.IsValid)
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
                    usuario.Roles.Add(new UsuarioRol { IdUsuario = usuario.IdUsuario, IdRol = 2 });
                    ctx.Usuario.Add(usuario);
                    await ctx.SaveChangesAsync();
                    usuario.Contraseña = "";
                    usuario.Salt = "";
                    return Ok();
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
                return Ok();
            }
        }
        //_----------------------------------------------------------------------------------------------------------------
        [Authorize(Roles = "Operador")]
        [HttpGet("{id}")]

        public async Task<IActionResult> AgregarProducto(int id)
        {
            var result = await ctx.Comercio.Select(x => new { IdComercio = x.IdComercio, IdUsuario = x.IdUsuario }).Where(x => x.IdUsuario == id).ToListAsync();
            var resultProducto = result.Select(x => new Comercio { IdComercio = x.IdComercio }).ToList();
            foreach (var item in resultProducto)
            {
                ViewBag.IdComercio = item.IdComercio;
            }
            return View();
        }

        public async Task<IActionResult> CargarProductos(int id)
        {
            var result = await ctx.Comercio.Select(x => new { IdComercio = x.IdComercio, IdUsuario = x.IdUsuario }).Where(x => x.IdUsuario == id).ToListAsync();
            var resultProducto = result.Select(x => new Comercio { IdComercio = x.IdComercio }).ToList();
            Producto Producto = new Producto();
            if (resultProducto.Count==0)
            {
                return RedirectToAction("AgregarProducto");
            }
            else
            {
                foreach (var item in resultProducto)
                {

                    List<Producto> Productos = await ctx.Producto.Where(x => x.IdComercio == item.IdComercio).ToListAsync();
                    ViewBag.Productos = Productos;
                }
                return View(Producto);
            }

        }
        [BindProperty]
        public Producto Productos { get; set; }
        [HttpPost]
        public async Task<IActionResult> AgregarProductos()
        {
            var result = await ctx.Producto.Where(x => x.NombreProducto == Productos.NombreProducto).SingleOrDefaultAsync();
            if (result != null)
            {
                //Usamos LinQ del badrequest para mostrar el error.
                return BadRequest(new JObject()
                {
                    {"StatusCode", 404 },
                    {"Message","Este producto ya existe. Ingrese uno nuevo." }
                });
            }
            else
            {
                var _Producto = await ctx.Producto.Where(x => x.IdProducto == Productos.IdProducto).AnyAsync();
                if (!_Producto)
                {
                    ctx.Producto.Add(Productos);
                }else
                {
                    ctx.Producto.Attach(Productos);
                    ctx.Entry(Productos).State = EntityState.Modified;
                }

                await ctx.SaveChangesAsync();
                return Ok();
            }
        }
        public async Task<IActionResult> Modificar(int id)
        {
            var Producto = await ctx.Producto.FindAsync(id);
            if (Producto == null)
            {
                return RedirectToAction("CargarProductos");
            }
            return View(Producto);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var _Producto = await ctx.Producto.FindAsync(id);
            if (_Producto == null)
            {
                return RedirectToAction("CargarProductos");
            }
            else
            {
                ctx.Producto.Remove(_Producto);
                await ctx.SaveChangesAsync();
                return RedirectToAction("CargarProductos");
            }
        }
    }
}
