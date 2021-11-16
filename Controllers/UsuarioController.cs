using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    public class UsuarioController : Controller
    {
        CodeStackCTX ctx;

        public UsuarioController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Comercio = await ctx.Comercio.ToListAsync();
            Comercio comercio = new Comercio();
            return View(comercio);
        }
       
        [HttpGet("id")]
        public async Task<IActionResult> MostrarProductos(int id)
        {
            ViewBag.Producto = await ctx.Producto.Where(x=> x.IdComercio == id).ToListAsync();
            Producto producto = new Producto();
            return View(producto);
        }
        
    }
}
