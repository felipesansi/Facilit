using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class Clientes
    {
        public string id { get; set; }
        public string codigo { get; set; }
        public string nome { get; set; }
        public string fantasia { get; set; }
        public string tipo_pessoa { get; set; }
        public string cpf_cnpj { get; set; }
    }
}