using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class PagarePrestamo
    {
        [Key]
        public string Nombre { get; set; }
        public string identidad { get; set; }
        public string CLI_Direccion { get; set; }
        public decimal Monto { get; set; }
        public string Fec { get; set; }
        public int Tipo { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}