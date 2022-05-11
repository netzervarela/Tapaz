using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class EstudioSocioEconomico
    {
        [Key]
        public string Nombre { get; set; }
        public string CLI_Direccion { get; set; }
        public string CLI_Cel { get; set; }
        public decimal ESE_NumeroDependientes { get; set; }
        public decimal ESE_NegocioPropio { get; set; }
        public decimal ESE_Salario { get; set; }
        public decimal ESE_ActividadAgropecuaria { get; set; }
        public decimal ESE_Otros_Ingresos { get; set; }
        public decimal ESE_Renta { get; set; }
        public decimal ESE_ServiciosPublicos { get; set; }
        public decimal ESE_Prestamos { get; set; }
        public decimal ESE_Transporte { get; set; }
        public decimal ESE_Alimentacion { get; set; }
        public decimal ESE_Vestuario { get; set; }
        public decimal ESE_Otros_Egresos { get; set; }
        public decimal totalIngresos { get; set; }
        public decimal totalEgresos { get; set; }
        public string ESE_Observaciones { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}