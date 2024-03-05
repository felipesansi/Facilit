using Facilit.Models;
using Facilit.Models.ClienteTiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Facilit.Servicos
{
    public class RetornoTinyApi
    {
        public async Task<string> ListarProdutos(string token)
        {

            string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
            string formatoRetorno = "json";

          
            var cliente = new HttpClient();
            var requisicao = new HttpRequestMessage(HttpMethod.Get, $"https://api.tiny.com.br/api2/pdv.produtos.php?token={tokenTiny}&formato={formatoRetorno}");
            var resposta = await cliente.SendAsync(requisicao);
           
            if (resposta.IsSuccessStatusCode)
            {
                var respostaJson = await resposta.Content.ReadAsStringAsync();
                var obj_json = JsonSerializer.Deserialize<ProdutoTiny>(respostaJson);
            }
           
            var resultadoConvertidoEmString = await resposta.Content.ReadAsStringAsync();
            return resultadoConvertidoEmString;
        }
        public async Task<string> ListarClientes(string token)
        {

            string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
            string formatoRetorno = "json";

            var cliente = new HttpClient();
            var requisicao = new HttpRequestMessage(HttpMethod.Get, $"https://api.tiny.com.br/api2/contatos.pesquisa.php?token=02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a&formato=json{tokenTiny}&formato={formatoRetorno}");
            var resposta = await cliente.SendAsync(requisicao);
            if (resposta.IsSuccessStatusCode)
            {
                var respostaJson = await resposta.Content.ReadAsStringAsync();
                var obj_json = JsonSerializer.Deserialize<ClienteTiny>(respostaJson);
            }


            var resultadoConvertidoEmString = await resposta.Content.ReadAsStringAsync();
            return resultadoConvertidoEmString;
        }
    }
}
