using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    public class OperadorController : Controller
    {
        CodeStackCTX ctx;
        public OperadorController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        [Authorize(Roles = "Operador")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> AgregarProducto(int id)
        {
            var result = await ctx.Comercio.Where(x => x.IdUsuario == id).ToListAsync();
            var resultProducto = result.Select(x => new Comercio { IdComercio = x.IdComercio }).ToList();
            foreach (var item in resultProducto)
            {
                ViewBag.IdComercio = item.IdComercio;
            }
            return View();
        }

        public async Task<IActionResult> CargarProductos(int id)
        {
            var result = await ctx.Comercio.Where(x => x.IdUsuario == id).ToListAsync();
            ViewBag.Comercio = result;
            var resultProducto = result.Select(x => new Comercio { IdComercio = x.IdComercio }).ToList();
            Producto Producto = new Producto();
            if (resultProducto.Count == 0)
            {
                return RedirectToAction("AgregarProducto", "Operador");
            }
            else
            {
                foreach (var item in resultProducto)
                {
                    List<Producto> productos= await ctx.Producto.Where(x => x.IdComercio == item.IdComercio).ToListAsync();
                    ViewBag.Productos = productos;
                }
                return View(Producto);
            }
        }
        //Metódos para agregar o eliminar producto
        [BindProperty]
        public Producto Productos { get; set; }
        [HttpPost]
        public async Task<IActionResult> AgregarProductos(FileUpload fileUpload)
        {
            var result = await ctx.Producto.AsNoTracking().Where(x => x.Nombre == Productos.Nombre).SingleOrDefaultAsync();
            int numero;
            if (result == null)
            {
                numero = 0;
            }
            else
            {
                numero = result.IdProducto;
            }
            var _Producto = await ctx.Producto.Where(x => x.IdProducto == numero).AnyAsync();
            if (_Producto != false)
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
                if (!_Producto)
                {
                    if (!ModelState.IsValid)
                    {
                        if (fileUpload.Foto != null)
                        {
                            var img = fileUpload.Foto;

                            var filename = Path.GetFileName(fileUpload.Foto.FileName);
                            var contentype = fileUpload.Foto.ContentType;
                            byte[] bytes = null;
                            using (var target = new MemoryStream())
                            {
                                img.CopyTo(target);
                                bytes = target.ToArray();
                            }
                            Productos.Nombre = fileUpload.Nombre;
                            Productos.Cantidad = fileUpload.Cantidad;
                            Productos.Descripcion = fileUpload.Descripcion;
                            Productos.Precio = fileUpload.Precio;
                            Productos.IdComercio = fileUpload.IdComercio;
                            Productos.Foto = bytes;
                            Productos.Fototipo = contentype;

                            ctx.Producto.Add(Productos);
                            await ctx.SaveChangesAsync();
                            return RedirectToAction("Index", "Operador");
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    Productos.Foto = result.Foto;
                    Productos.Fototipo = result.Fototipo;
                    ctx.Producto.Attach(Productos);
                    ctx.Entry(Productos).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    
                }
                return RedirectToAction("AgregarProducto", "Operador");
            }
        }
        public async Task<IActionResult> Modificar(int id)
        {
            var Producto = await ctx.Producto.FindAsync(id);
            ViewBag.Producto = Producto.IdComercio;
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

        public byte[] GetImage (string imagen)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(imagen))
            {
                bytes = Convert.FromBase64String(imagen);
            }
            return bytes;
        }
    }
}
