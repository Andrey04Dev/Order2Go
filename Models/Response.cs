using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.Models
{
    public class Response
    {
        public string mensaje { get; set; }
        public bool estado { get; set; }
        public dynamic resultado { get; set; }
    }
}
