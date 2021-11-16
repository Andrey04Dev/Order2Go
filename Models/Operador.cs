using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Models
{
    [Table("tbl_operador")]
    public class Operador
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Escriba su nombre completo")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        [RegularExpression("^[a-zA-Z\u00E0-\u00FC\u00f1\u00d1 ]{2,254}", ErrorMessage = "Solo debe ingresar letras")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Escriba sus apellidos")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        [RegularExpression("^[a-zA-Z\u00E0-\u00FC\u00f1\u00d1 ]{2,254}", ErrorMessage = "Solo debe ingresar letras")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Escriba el correo. No deje espacios en blanco")]
        [EmailAddress(ErrorMessage = "Escriba un correo válido")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Escriba la contraseña. No se puede dejar en blanco")]
        [RegularExpression("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^(.{8,15})$", ErrorMessage = "La contraseña debe tener al entre 8 y 16 caracteres, al menos un dígito, al menos una minúscula y al menos una mayúscula.Puede tener otros símbolos.")]
        public string Contraseña { get; set; }
        public string Salt { get; set; }
    }
}
