using Facilit.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Facilit.Servicos
{
    public class ProdutoTinyApi
    {
        public async Task<Produto> interacao(int id)
        {
            HttpClient client = new HttpClient();
            var json_string = await client.GetStringAsync($"https://api.tiny.com.br/api2/pedido.obter.php{id}");



            var jsonObject = JsonConvert.DeserializeObject<Produto>(json_string);

            if (jsonObject != null)
            {
                return jsonObject;
            }
            else
            {
                return new Produto
                {
                    Validacao = true
                };

            }


        }
    }
}