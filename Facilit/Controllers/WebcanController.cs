using Facilit.Models;
using Facilit.Servicos;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Facilit.Controllers
{
    public class WebcanController : Controller
    {
        string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";


        public async Task <ActionResult> Registro()
        {
           RetornoTinyApi produto = new RetornoTinyApi();
            var produtos = await produto.ListarProdutos(tokenTiny);
            var dropdown_produto = produtos.retorno.produtos.Select(s => new { Id = s.id, Produto = s.descricao + " | " + s.tipoVariacao }).ToList();
            ViewBag.Produtos = new SelectList( dropdown_produto, "Id", "Produto");
            //clientes

            RetornoTinyApi cliente = new RetornoTinyApi();
            var clientes = await cliente.ListarClientes(tokenTiny);
            var dropdown_cliente = clientes.retorno.contatos.Select(s => new {Id = s.contato.id, cliente =s.contato.nome }).ToList();
            ViewBag.Clientes = new SelectList(dropdown_cliente, "Id", "Cliente");
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



    }
   

}
