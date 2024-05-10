using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class EtiquetasTiny
    {
        public string formato_envio { set; get; }
        public string expedicaoSelecionada { get; set; }
        public int id_expedicao{ get; set; }
        public string NomeExpedicao { get; set; }
    }
}