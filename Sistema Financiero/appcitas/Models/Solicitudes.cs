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
    public class Solicitudes
    {
        [Key]
        public int PRES_Codigo { get; set; }
        public int PRES_Codigo_CLI     { get; set; }
        public string PRES_Fecha_Solicitud { get; set; }
        public int PRES_mto_Solicitado { get; set; }
        public string PRES_Fecha_Aprob { get; set; }
        public string PRES_mto_Aprobado { get; set; }
        public int PRES_Plazo_Meses { get; set; }
        public decimal PRES_Porc_Interes { get; set; }
        public int PRES_Estado { get; set; }
        public string PRES_Observaciones { get; set; }
        public int PRES_Garantia { get; set; }
        public string PRES_Agrego { get; set; }
        public string PRES_Fecha_Agrego { get; set; }

        public int CLI_Codigo { get; set; }
        public string CLI_Identidad { get; set; }
        public string CLI_Nombre { get; set; }

        public string GAR_Codigo { get; set; }
        public string GAR_Descripcion { get; set; }
        public string DES_Descripcion { get; set; }
        public int PRES_Frec_Pago { get; set; }
        public int PRES_Tipo_Cuota { get; set; }

        // datos generales estudio socioeconomico
        public string Personas { get; set; }

        // ingresos estudio socioecnonomico
        public string NegocioPropio { get; set; }
        public string Salario { get; set; }
        public string Finca { get; set; }
        public string Otros { get; set; }

        // egresos estudio socioeconomico
        public string Renta { get; set; }
        public string ServicioPublico { get; set; }
        public string Prestamo { get; set; }
        public string Transporte { get; set; }
        public string Alimentacion { get; set; }
        public string Vestuario { get; set; }
        public string Otros1 { get; set; }

        // datos finales estudio socioeconomico
        public string Observaciones { get; set; }



        public int Accion { get; set; }
        public string Mensaje { get; set; }


    }
}