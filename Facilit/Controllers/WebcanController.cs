using Facilit.Models;
using Facilit.Servicos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Facilit.Controllers
{
    public class WebcanController : Controller
    {
        public ActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SalvarFoto(string dados_imagem)
        {
            byte[] vet_bytes = Convert.FromBase64String(dados_imagem);
            string caminho_diretorio = Server.MapPath("~/Fotos");
            string caminho_imagem = Path.Combine(caminho_diretorio, "Produto_foto.jpg");

          
            if (!Directory.Exists(caminho_diretorio))
          
            {
                Directory.CreateDirectory(caminho_diretorio);
            }


            

            System.IO.File.WriteAllBytes(caminho_imagem, vet_bytes);
            return Json(new { sucesso = true, mensagem = "Foto salva com sucesso" });

        }

        //public async Task<ActionResult> Registro()
        //{ 
        //    string token = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
        //ProdutoTinyApi api = new ProdutoTinyApi();
        //    string retorno_json = await api.ListarProdutos(token);


        //    List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(retorno_json);


        //    ViewBag.Produtos = new SelectList(produtos, "Id", "Nome");

        //    return View();
        //}
    }
   

}
