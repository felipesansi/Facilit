using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class pdf
    {
        public int id_fotos { get; set; }
        public string nome_completo { get; set; }
        public string nome_produto { get; set; }
        public string nome_cliente { get; set; }
        public DateTime data_tirada { get; set; }


    }
}