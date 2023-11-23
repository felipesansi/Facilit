using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Facilit.Scripts
{
    public class ProdutoTynyServicos
    {
        private const string BaseURL = "https://api.tiny.com.br/api2/pedido.obter.php/";

        public async Task<produtoResponse> ConsutaPrutoAsnync(int id)
        {
            using(var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"{BaseURL}{id}/json");
                return JsonConvert.DeserializeObject<produtoResponse>(response);
            }
        }
    }
}