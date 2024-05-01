namespace Facilit.Models
{
    public class NotaFiscalLink
    {
        public Retorno retorno { get; set; }

        public class Retorno
        {
            public string status_processamento { get; set; }
            public string status { get; set; }
            public string codigo_erro { get; set; }
            public string link_nfe { get; set; }
        }
    }
}