using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        CodeStackCTX ctx;

        public ProductoController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        //[Authorize(Roles = "Operador")]
        
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
