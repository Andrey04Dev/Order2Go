using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Models
{
    [Table("tbl_comercios")]
    public class Comercio
    {
        [Key]
        public int IdComercio { get; set; }
        [Required(ErrorMessage = "Escriba el nombre del comercio")]
        [Display(Name ="Nombre del comercio:")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        [RegularExpression("^[a-zA-Z\u00E0-\u00FC\u00f1\u00d1 ]{2,254}", ErrorMessage = "Solo debe ingresar letras")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción:")]
        [Required(ErrorMessage = "Escriba la descripción del comercio")]
        [MaxLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres")]
        public string Descripcion { get; set; }
        [Display(Name ="Operador:")]
        [ForeignKey("IdUsuario")]
        [Required(ErrorMessage ="Debe selecionar un operador")]
        public int IdUsuario { get; set; }  
    }
}
