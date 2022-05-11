using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class DepositosPlazoFijo
    {
        [Key]
        public int ClienteId { get; set; }
        [Key]
        public int DPF_Codigo { get; set; }
        public string DPF_Fecha_Apert { get; set; }
        public string DPF_Monto { get; set; }
        public string DPF_Saldo { get; set; }
        public string DPF_Plazo { get; set; }
        public string DPF_Tasa_interes { get; set; }
        public int DPF_Tipo { get; set; }
        public string TDP_Descripcion { get; set; }
        public string DPF_Beneficiario_1 { get; set; }
        public string DPF_ID_Bene_1 { get; set; }
        public string DPF_Porc_1 { get; set; }
        public string DPF_Beneficiario_2 { get; set; }
        public string DPF_ID_Bene_2 { get; set; }
        public string DPF_Porc_2 { get; set; }
        public string DPF_Beneficiario_3 { get; set; }
        public string DPF_ID_Bene_3 { get; set; }
        public string DPF_Porc_3 { get; set; }
        public int DPF_Estado { get; set; }
        public string DPF_Vencido { get; set; }
        public string DPF_Observacion { get; set; }
        public string DPF_Pago_intereses { get; set; }
        public string Nombre { get; set; }
        public int Accion { get; set; }
        public string FechaVencimiento { get; set; }
        public string Mensaje { get; set; }
       
    }
}