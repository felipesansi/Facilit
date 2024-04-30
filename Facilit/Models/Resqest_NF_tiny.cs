using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class Resqest_NF_tiny
    {
        public string TipoNota { get; set; } 
        public string Numero { get; set; } 
        public string Cliente { get; set; } 
        public string CPF_CNPJ { get; set; } 
        public string DataInicial { get; set; }
        public string DataFinal { get; set; } 
        public string Situacao { get; set; } 
        public string NumeroEcommerce { get; set; } 
      
    }
}