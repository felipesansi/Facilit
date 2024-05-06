using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class PesquisaExpedicao
    {

        public class Rootobject
        {
            public Retorno retorno { get; set; }
        }

        public class Retorno
        {
            public string status_processamento { get; set; }
            public string status { get; set; }
            public int pagina { get; set; }
            public int numero_paginas { get; set; }
            public Expedico[] expedicoes { get; set; }
        }

        public class Expedico
        {
            public Expedicao expedicao { get; set; }
        }

        public class Expedicao
        {
            public string id { get; set; }
            public string tipoObjeto { get; set; }
            public string idObjeto { get; set; }
            public string idAgrupamento { get; set; }
            public string situacao { get; set; }
            public string dataEmissao { get; set; }
            public string formaEnvio { get; set; }
            public string identificacao { get; set; }
            public string qtdVolumes { get; set; }
            public string valorDeclarado { get; set; }
            public string possuiValorDeclarado { get; set; }
            public string pesoBruto { get; set; }
            public string codigoRastreamento { get; set; }
            public string urlRastreamento { get; set; }
            public string possuiAR { get; set; }
            public Embalagem embalagem { get; set; }
            public Destinatario destinatario { get; set; }
            public Transportadora transportadora { get; set; }
            public Formafrete formaFrete { get; set; }
        }

        public class Embalagem
        {
            public string tipo { get; set; }
            public string altura { get; set; }
            public string largura { get; set; }
            public string comprimento { get; set; }
            public string diametro { get; set; }
        }

        public class Destinatario
        {
            public string nome { get; set; }
            public string endereco { get; set; }
            public string numero { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string cep { get; set; }
            public string cidade { get; set; }
            public string uf { get; set; }
        }

        public class Transportadora
        {
            public object id { get; set; }
            public string nome { get; set; }
        }

        public class Formafrete
        {
            public int id { get; set; }
            public string descricao { get; set; }
        }

    }
}