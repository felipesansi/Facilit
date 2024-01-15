using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    [JsonObject]
    public class Produto
    {
        public string Token{ get; set; }
        public string Formato{ get; set; }
        public int Id{ get; set; }
        public int Status_processamento { get; set; }
        public string Status{ get; set; }
        public int paginas { get; set; }
        public int numeros_paginas { get; set; }

        public string Descricao { get; set; }
        public int Codigo{ get; set; }
        public string Tipo { get; set; }
        public double Preco { get; set; }


    }
}