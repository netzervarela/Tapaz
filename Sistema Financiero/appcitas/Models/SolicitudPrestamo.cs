using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class SolicitudPrestamo
    {
        public string Nombre { get; set; }
        public string Fecha { get; set; }
        public string Lugar { get; set; }
        public string Identidad { get; set; }
        public string EstadoCivil { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Profesion { get; set; }
        public string Celular { get; set; }
        public string MontoSolicitado { get; set; }
        public string TasaInteres { get; set; }
        public string Plazo { get; set; }
        public string TipoGarantia { get; set; }
        public string FormaPago { get; set; }
        public string CuotaMensual { get; set; }
        public string FechaDesembolso { get; set; }
        public string MontoAprobado { get; set; }
        public string CreditoNuevo { get; set; }
        public string ObjetivoPrestamo { get; set; }
        public string NomRef1 { get; set; }
        public string NomRef2 { get; set; }
        public string CelRef1 { get; set; }
        public string CelRef2 { get; set; }
        public string dirTrabajo { get; set; }
        public string Observaciones { get; set; }
        public string FechaAprobacion { get; set; }
    }
}