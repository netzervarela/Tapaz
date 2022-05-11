using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class Avalaes
    {
        [Key]
        public int PrestamoId { get; set; }
        public int AVE_Codigo_pres { get; set; }
        public string AVA_Identidad { get; set; }
        public string AVA_Nombre { get; set; }
        public string AVA_Telefono { get; set; }
        public string AVA_Celular { get; set; }
        public string AVA_Direccion { get; set; }
        public int AVA_Num { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}