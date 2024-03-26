using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class EquiquetasTiny // ainda fazendo
    {
        public class Retorno
        {
        public string   status_processamento { get; set; }
        public string status { get; set; }
         public Links[] link { get; set; }
        }
        public class Links 
        {
            public string link  { get; set; }
        }
        

    }

}