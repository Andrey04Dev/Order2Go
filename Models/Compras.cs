using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Models
{
    [Table("tbl_compras")]
    public class Compras
    {
        [Key]
        public int IdCompra { get; set; }
        public int IdProducto { get; set; }
        public int IdComercio { get; set; }
        public int IdUsuario { get; set; }
        [Required(ErrorMessage ="Debe de ingresar la fecha")]
        public string Fecha { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CodigoFactura { get; set; }
        public int Precio { get; set; }
        [Required(ErrorMessage = "Debe de ingresar la cantidad de platillos")]
        public int CantidadProductos { get; set; }
        [ForeignKey("IdProducto")]
        public virtual Producto Productos { get; set; }
        [ForeignKey("IdComercio")]
        public virtual Comercio Comercio { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
