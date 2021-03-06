using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Models
{
    public class FileUpload
    {
        [Required(ErrorMessage = "Escriba su nombre completo")]
        /*[MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        [RegularExpression("^[a-zA-Z\u00E0-\u00FC\u00f1\u00d1 ]{2,254}", ErrorMessage = "Solo debe ingresar letras")]*/
        [Display(Name = "Nombre del Producto")]
        public string Nombre { get; set; }
        [Display(Name = "Cantidad:")]
        /*[MaxLength(50, ErrorMessage = "El número de producto máximos es de 50 caracteres")]*/
        [Required(ErrorMessage = "Escriba un cantidad")]
        public int Cantidad { get; set; }
        [Display(Name = "Descipción del Producto:")]
        [Required(ErrorMessage = "Escriba su nombre completo")]
        /*[MaxLength(50, ErrorMessage = "La descripción no puede exceder los 50 caracteres")]
        [RegularExpression("^[a-zA-Z\u00E0-\u00FC\u00f1\u00d1 ]{2,254}", ErrorMessage = "Solo debe ingresar letras")]*/
        public string Descripcion { get; set; }
        [Display(Name = "Precio:")]
        [Required(ErrorMessage = "Escriba el precio del producto")]
        public int Precio { get; set; }
        [Display(Name = "Id del Comercio:")]
        public int IdComercio { get; set; }
        [Display(Name = "Foto del producto:")]
        public IFormFile Foto { get; set; }
    }
}
