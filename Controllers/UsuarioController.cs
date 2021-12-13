using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Order2Go.DataContext;
using Proyecto_Order2Go.Helpers;
using Proyecto_Order2Go.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        CodeStackCTX ctx;
        Response res = new Response();
        SessionHelpers helpers = new SessionHelpers();
        public UsuarioController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }
        [Authorize(Roles = "Usuario")]
        [HttpGet("id")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Comercio = await ctx.Comercio.ToListAsync();
            Comercio comercio = new Comercio();
            return View(comercio);
        }
        public async Task<IActionResult> VerFactura(int id)
        {
            try
            {
                var FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                var result = await ctx.Compras.Include(x => x.Productos).Include(x => x.Comercio).Include(x => x.Usuario)
                    .Where(x => x.IdUsuario == id && x.Fecha == FechaHoy).AsNoTracking().ToListAsync();
                ViewBag.Compras = result;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.StackTrace);
            }
            return View();
        }

        public async Task<IActionResult> MostrarProductos(int id)
        {
            ViewBag.Producto = await ctx.Producto.Where(x => x.IdComercio == id).ToListAsync();
            Producto producto = new Producto();
            return View(producto);
        }
        public async Task<IActionResult> AgregarProducto(int id)
        {

            ViewBag.Producto = await ctx.Producto.Where(x => x.IdProducto == id).ToListAsync();
            //Para jalar 2 vista en ASp.net
            Tuple<Compras, Producto> Model = new Tuple<Compras, Producto>(new Compras(), new Producto());
            return View(Model);
        }
        public async Task<IActionResult> EnviarCorreoFactura(int id)
        {
            var FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            var resultList = await ctx.Compras.Include(x => x.Productos).Include(x => x.Comercio).Include(x => x.Usuario)
                .Where(x => x.IdUsuario == id && x.Fecha == FechaHoy).ToListAsync();
            var correoOcupado = "";
            string bodymail = "";
            foreach (var result in resultList)
            {
                correoOcupado = result.Usuario.Correo;
                bodymail += "<tr>" +
                                "<td style=\"1px solid black;\">" + result.CodigoFactura + "</td>" +
                                "<td style=\"1px solid black;\">" + result.Fecha + "</td>" +
                                "<td style=\"1px solid black;\">" + result.Comercio.Nombre + "</td>" +
                                "<td style=\"1px solid black;\">" + result.Productos.Nombre + "</td>" +
                                "<td style=\"1px solid black;\">" + result.CantidadProductos + "</td>" +
                                "<td style=\"1px solid black;\">" + result.Precio + "</td>" +
                                "<td style=\"1px solid black;\">" + (result.CantidadProductos * result.Precio) + "</td>" +
                             "</tr>";
            }
            string MailBody = "<h2 style= \"text-align: center;margin: 10px;\">Factura de Productos</h2>" +
                    "<p style=\"text-align: center;margin: 10px;\">Le hacemos envio de la factura electrónica. Muchas gracias por preferirnos.</p>" +
                    "<div style = \"display: flex;justify-content: center;\">" +
                        "<table style=\"border-collapse: collapse; text-align: center; width: 100%; padding: 10px; margin: 10px;font-family: Arial, Helvetica, sans-serif;letter-spacing: 1.5px; font-size: 18px;border: 1px solid black;\">" +
                            "<thead style =\"background-color: #0069d9; color:white; font-weight: bold; border: 1px solid black;\">" +
                                "<tr>" +
                                    "<th style=\"1px solid black;\">Número Factura</th>" +
                                    "<th style=\"1px solid black;\">Fecha de Compra</th>" +
                                    "<th style=\"1px solid black;\">Comercio</th>" +
                                    "<th style=\"1px solid black;\">Producto</th>" +
                                    "<th style=\"1px solid black;\">Cantidad</th>" +
                                    "<th style=\"1px solid black;\">Precio</th>" +
                                    "<th style=\"1px solid black;\">Total de compra</th>" +
                                "</tr>" +
                            "</thead>" +
                            "<tbody>" +
                            bodymail+
                            "</tbody>" +
                        "</table>" +
                    "</div>";
            string fromEmail = "pirulo123net@gmail.com";
            string Password = "Pirulo04041993";
            string Subject = "Factura de Productos";
            string EmailTitle = "Factura Electrónica";

            MailMessage correo = new MailMessage(new MailAddress(fromEmail, EmailTitle), new MailAddress(correoOcupado));
            correo.Subject = Subject;
            correo.Body = MailBody;
            correo.IsBodyHtml = true;
            correo.BodyEncoding = System.Text.Encoding.UTF8;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = fromEmail;
            credential.Password = Password;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;

            smtp.Send(correo);
            ViewBag.EmailCorrecto = "El correo se ha enviado correctamente";
            return RedirectToAction("VerFactura", "Usuario", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> AgregarProductosCarrito([Bind(Prefix = "Item1")] Compras Compras)
        {
            if (!ModelState.IsValid)
            {
                res.estado = false;
                res.mensaje = "Rellene los campos solicitados.";
                return Json(res);
            }
            //var _Compras = await ctx.Compras.Where(x => x.CodigoFactura == ).SingleOrDefaultAsync();
            ctx.Compras.Add(Compras);
            await ctx.SaveChangesAsync();
            return RedirectToAction("Index", "Usuario");
        }
        public async Task<IActionResult> EliminarProductoLista(int id)
        {
            var _Compra = await ctx.Compras.FindAsync(id);
            if (_Compra == null)
            {
                return BadRequest("No se ha encontrado ese producto en la lista");
            }
            else
            {
                ctx.Compras.Remove(_Compra);
                await ctx.SaveChangesAsync();
                return RedirectToAction("VerFactura", "Usuario", new { id = _Compra.IdUsuario });
            }
        }

    }
}
