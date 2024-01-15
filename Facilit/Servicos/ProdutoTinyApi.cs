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
        public async Task<string> ListarProdutos(string token)
        {

            string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
            string formatoRetorno = "json";

          
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.tiny.com.br/api2/pdv.produtos.php?token={tokenTiny}&formato={formatoRetorno}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var resultadoConvertidoEmString = await response.Content.ReadAsStringAsync();
            return resultadoConvertidoEmString;
        }
        public async Task<string> ListarClientes(string token)
        {

            string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
            string formatoRetorno = "json";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.tiny.com.br/api2/contatos.pesquisa.php?token=02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a&formato=json{tokenTiny}&formato={formatoRetorno}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var resultadoConvertidoEmString = await response.Content.ReadAsStringAsync();
            return resultadoConvertidoEmString;
        }
    }
}
