using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class Socioeconomico
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string NumDependientes { get; set; }
        public string Negocio { get; set; }
        public string Renta { get; set; }
        public string Salario { get; set; }
        public string ServiciosPub { get; set; }
        public string ActividadAgricola { get; set; }
        public string Prestamos { get; set; }
        public string Transporte { get; set; }
        public string otrosEgresos { get; set; }
        public string otrosIngresos { get; set; }
        public string TotalIngresos { get; set; }
        public string TotalEgresos { get; set; }
        public string CapacidadPago { get; set; }
        public string Observaciones { get; set; }
        public string Alimentacion { get; set; }
        public string Vestimenta { get; set; }
    }
}