using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class ContratoPrestamo
    {
        [Key]
        public string Nombre { get; set; }
        public string identidad { get; set; }
        public string CLI_Direccion { get; set; }
        public decimal Monto { get; set; }
        public string Fec { get; set; }
        public int PRES_Plazo_Meses { get; set; }
        public int NumCuotas { get; set; }
        public decimal PRES_Porc_Interes { get; set; }
        public string DES_Descripcion { get; set; }
        public string GAR_Descripcion { get; set; }
        public int IdPres { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}