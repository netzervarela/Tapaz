using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class SolicitudCredito
    {
        [Key]
        public string Nombre { get; set; }
        public string CLI_Identidad { get; set; }
        public string CLI_Direccion { get; set; }
        public string Estado_Civil { get; set; }
        public string CLI_Cel { get; set; }
        public decimal PRES_Mto_Solicitado { get; set; }
        public decimal PRES_Porc_Interes { get; set; }
        public decimal PRES_Mto_Aprobado { get; set; }
        public int PRES_Plazo_Meses { get; set; }
        public string DES_Descripcion { get; set; }
        public string GAR_Descripcion { get; set; }
        public string FormaPago { get; set; }
        public string PRES_Observaciones { get; set; }
        public string Fecha { get; set; }
        public string PRES_Fecha_Aprob { get; set; }
        public string REF_Nombre { get; set; }
        public string REF_Celular { get; set; }
        public int REF_Num { get; set; }
        public string PRO_Descripcion { get; set; }
        public string CLI_Direc_Trabajo { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}