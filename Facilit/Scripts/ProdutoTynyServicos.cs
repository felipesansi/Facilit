using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Facilit.Models;

namespace Facilit.Scripts
{
    public class ProdutoTynyServicos
    {

        public async Task<Produto> interacao(int id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync($"https://api.tiny.com.br/api2/pedido.obter.php{id}");

            var json_string = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject <Produto>(json_string);

            if (jsonObject != null)
            {
                return jsonObject;
            }
            else
            {
                new Produto
                {
                    Validacao = true
                };

            }


        }

    }
}