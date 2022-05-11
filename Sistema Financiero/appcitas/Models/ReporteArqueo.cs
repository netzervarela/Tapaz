using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class ReporteArqueo
    {
        [Key]
        public string NombreCajero { get; set; }
        public string CA_Fecha { get; set; }
        public decimal CA_Valor_Inicial_Dia { get; set; }
        public decimal CA_Valor_Recib { get; set; }
        public decimal CA_Valor_Entrega { get; set; }
        public decimal CA_Valor_Recib_Supervisor { get; set; }
        public decimal CA_Valor_Entrega_Supervisor { get; set; }
        public int CA_B_1 { get; set; }
        public int CA_B_2 { get; set; }
        public int CA_B_5 { get; set; }
        public int CA_B_10 { get; set; }
        public int CA_B_20 { get; set; }
        public int CA_B_50 { get; set; }
        public int CA_B_100 { get; set; }
        public int CA_B_500 { get; set; }
        public decimal CA_M_1 { get; set; }
        public decimal CA_M_2 { get; set; }
        public decimal CA_M_5 { get; set; }
        public decimal CA_M_10 { get; set; }
        public decimal CA_M_20 { get; set; }
        public decimal CA_M_50 { get; set; }
        public int Tot_B1 { get; set; }
        public int Tot_B2 { get; set; }
        public int Tot_B5 { get; set; }
        public int Tot_B10 { get; set; }
        public int Tot_B20 { get; set; }
        public int Tot_B50 { get; set; }
        public int Tot_B100 { get; set; }
        public int Tot_B500 { get; set; }
        public decimal Tot_M1 { get; set; }
        public decimal Tot_M2 { get; set; }
        public decimal Tot_M5 { get; set; }
        public decimal Tot_M10 { get; set; }
        public decimal Tot_M20 { get; set; }
        public decimal Tot_M50 { get; set; }
        public decimal SaldoEnCaja { get; set; }
        public int TotalBilletes { get; set; }
        public decimal TotalMonedas { get; set; }
        public decimal TotalArqueo { get; set; }
        public decimal CA_Diferencia { get; set; }
        public decimal Sobrante { get; set; }
        public decimal Faltante { get; set; }
        public string EstadoCajero { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}