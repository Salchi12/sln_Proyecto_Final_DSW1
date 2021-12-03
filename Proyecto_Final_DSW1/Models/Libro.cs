using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Final_DSW1.Models
{
    public class Libro
    {
        public int idlibro { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public int idcategoria { get; set; }
        public Int16 stock { get; set; }
    }
}