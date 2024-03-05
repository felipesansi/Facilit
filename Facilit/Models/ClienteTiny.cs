using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models.ClienteTiny
{

    public class ClienteTiny
    {
        public Retorno retorno { get; set; }
    }

    public class Retorno
    {
        public string status_processamento { get; set; }
        public string status { get; set; }
        public int pagina { get; set; }
        public int numero_paginas { get; set; }
        public Contato[] contatos { get; set; }
    }

    public class Contato
    {
        public Contato1 contato { get; set; }
    }

    public class Contato1
    {
        public string id { get; set; }
        public string codigo { get; set; }
        public string nome { get; set; }
        public string fantasia { get; set; }
        public string tipo_pessoa { get; set; }
        public string cpf_cnpj { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string email { get; set; }
        public string fone { get; set; }
        public int id_lista_preco { get; set; }
        public object id_vendedor { get; set; }
        public string nome_vendedor { get; set; }
        public string situacao { get; set; }
        public string data_criacao { get; set; }
    }

}