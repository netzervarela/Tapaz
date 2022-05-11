using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class RQ1
    {
        [Key]
        public string Nombre { get; set; }
        public string Monto { get; set; }
    }
}