using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Models.ViewModel
{
    public class UsuarioVM
    {
        [Required(ErrorMessage = "Escriba el nombre.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Escriba la contraseña.")]
        public string Contraseña { get; set; }
    }
}
