
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Order2Go.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    public class ComercioController : Controller
    {
        CodeStackCTX ctx;

        public ComercioController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        public async Task<IActionResult> Index()
        {
            return Ok(await ctx.Comercio.Include("Roles.Rol").ToListAsync());
        }
    }
}
