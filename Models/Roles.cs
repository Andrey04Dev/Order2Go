using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Models
{
    [Table("tbl_roles")]
    public partial class Roles
    {
        [Key]
        public int IdRole { get; set; }
        public string  Role { get; set; }
    }
}
