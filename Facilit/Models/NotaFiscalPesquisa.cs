using System.Collections.Generic;

namespace Facilit.Models
{
    public class NotaFiscalPesquisa
    {
        public Retorno retorno { get; set; }

        public class Retorno
        {
            public string status_processamento { get; set; }
            public string status { get; set; }
            public int pagina { get; set; }
            public int numero_paginas { get; set; }
            public Notas_Fiscais[] notas_fiscais { get; set; }
        }

        public class Notas_Fiscais
        {
            public Nota_Fiscal nota_fiscal { get; set; }
        }

        public class Nota_Fiscal
        {
            public string id { get; set; }
            public string tipo { get; set; }
            public string serie { get; set; }
            public string numero { get; set; }
            public object numero_ecommerce { get; set; }
            public string data_emissao { get; set; }
            public string nome { get; set; }
            public Cliente cliente { get; set; }
            public Endereco_Entrega endereco_entrega { get; set; }
            public Transportador transportador { get; set; }
            public string valor { get; set; }
            public string valor_produtos { get; set; }
            public string valor_frete { get; set; }
            public string id_vendedor { get; set; }
            public string nome_vendedor { get; set; }
            public string situacao { get; set; }
            public string descricao_situacao { get; set; }
            public string id_forma_frete { get; set; }
            public string id_forma_envio { get; set; }
            public string codigo_rastreamento { get; set; }
            public string url_rastreamento { get; set; }
        }

        public class Cliente
        {
            public string nome { get; set; }
            public string tipo_pessoa { get; set; }
            public string cpf_cnpj { get; set; }
            public string ie { get; set; }
            public string endereco { get; set; }
            public string numero { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string cep { get; set; }
            public string cidade { get; set; }
            public string uf { get; set; }
            public string fone { get; set; }
            public string email { get; set; }
        }

        public class Endereco_Entrega
        {
            public string tipo_pessoa { get; set; }
            public string cpf_cnpj { get; set; }
            public string endereco { get; set; }
            public string numero { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string cep { get; set; }
            public string cidade { get; set; }
            public string uf { get; set; }
            public string fone { get; set; }
            public string nome_destinatario { get; set; }
        }

        public class Transportador
        {
            public string nome { get; set; }
        }

    }
}