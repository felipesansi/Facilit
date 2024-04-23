using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
  
    public class ProdutoTiny
    {


        public Retorno retorno { get; set; }

        public class Retorno
        {
            public string status_processamento { get; set; }
            public string status { get; set; }
            public int pagina { get; set; }
            public int numero_paginas { get; set; }
            public Produto[] produtos { get; set; }
        }

        public class Produto
        {
            public string id { get; set; }
            public string descricao { get; set; }
            public string codigo { get; set; }
            public string tipo { get; set; }
            public string preco { get; set; }
            public string precoPromocional { get; set; }
            public string ncm { get; set; }
            public string unidade { get; set; }
            public string origem { get; set; }
            public string cest { get; set; }
            public string gtin { get; set; }
            public string gtinTributavel { get; set; }
            public string situacao { get; set; }
            public string categoria { get; set; }
            public string idCategoria { get; set; }
            public string classe_produto { get; set; }
            public string tipoVariacao { get; set; }
            public Tributacao tributacao { get; set; }
        }

        public class Tributacao
        {
            public string cfop { get; set; }
            public Pis pis { get; set; }
            public Cofins cofins { get; set; }
            public Simples simples { get; set; }
        }

        public class Pis
        {
            public string st { get; set; }
            public string aliquota { get; set; }
        }

        public class Cofins
        {
            public string st { get; set; }
            public string aliquota { get; set; }
        }

        public class Simples
        {
            public string st { get; set; }
            public string aliquotaAplicavelCredito { get; set; }
        }
        public class Produts
        {
            public int id { get; set; }
            public int codigo_tiny { get; set; }
            public string descricao { get; set; }
            public string unidade { get; set; }
            public DateTime data_atualizacao { get; set; }


        }

    }
  
}