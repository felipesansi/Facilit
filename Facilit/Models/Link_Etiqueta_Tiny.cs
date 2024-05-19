using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class Link_Etiqueta_Tiny
    {
        public Retorno retorno { get; set; }
        
        public class Retorno
        {
            public string status_processamento { get; set; }
            public string status { get; set; }
            public Link[] links { get; set; }
        }

        public class Link
        {
            public string link { get; set; }
        }

    }
}