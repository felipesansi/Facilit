using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class Clientes
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Fantasia { get; set; }
        public string Tipo_pessoa { get; set; }
        public string Cpf_cnpj { get; set; }
    }
}