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
            // Para teste, iremos deixar o valor do token fixo aqui dentro.
            string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
            string formatoRetorno = "json";

            // Requisição HttpRest
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.tiny.com.br/api2/pdv.produtos.php?token={tokenTiny}&formato={formatoRetorno}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Resultado tá sendo convertido em string já que ainda não existe o objeto que reflete com o real retorno.
            // TODO: Criar classes que sejam coerentes com o que a API retorna para fazer a conversão corretamente.
            var resultadoConvertidoEmString = await response.Content.ReadAsStringAsync();
            return resultadoConvertidoEmString;
        }
    }
}
