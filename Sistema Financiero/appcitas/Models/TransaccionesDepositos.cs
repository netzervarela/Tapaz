using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AccesoDatos;
using appcitas.Context;
using System.Data;
using System.Data.SqlClient;

namespace appcitas.Models
{
    public class TransaccionesDepositos
    {
        [Key]
        public int TRD_Codigo_DPF { get; set; }
        public string Fecha { get; set; }
        public decimal TRD_Deposito { get; set; }
        public decimal TRD_Retiro { get; set; }
        public decimal TRD_Interes { get; set; }
        public string TRD_Agrego { get; set; }
        public decimal Saldo { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}