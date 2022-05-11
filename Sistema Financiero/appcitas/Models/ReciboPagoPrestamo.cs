using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class ReciboPagoPrestamo
    {
        [Key]
        public int RE_Numero { get; set; }
        public string Nombre { get; set; }
        public int RE_Documento { get; set; }
        public string RE_Fecha { get; set; }
        public string Tipo { get; set; }
        public decimal PRES_Saldo { get; set; }
        public decimal RE_Total_Rec { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}