using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class ReciboOtros
    {
        [Key]
        public int re_numero { get; set; }
        public string servicio { get; set; }
        public string re_observacion { get; set; }
        public string re_fecha { get; set; }
        public decimal re_total_rec { get; set; }
        public string OT_Clave { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}