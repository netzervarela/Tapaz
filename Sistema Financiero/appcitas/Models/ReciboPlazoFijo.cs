using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class ReciboPlazoFijo
    {
        public string CodigoCliente { get; set; }
        public string ClienteId { get; set; }
        public string NumDeposito { get; set; }
        public double Cantidad { get; set; }
        public string Descripcion { get; set; }
        public Int32 CodigoDPF { get; set; }
        public DateTime Fecha { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}