using Facilit.Models;
using Facilit.Servicos;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Facilit.Controllers
{
    public class WebcanController : Controller
    {
        string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
        string mensagem;

        public async Task<ActionResult> Registro(int id)
        {
            RetornoTinyApi produto = new RetornoTinyApi();
            var produtos = await produto.ListarProdutos(tokenTiny);
            var dropdown_produto = produtos.retorno.produtos.Select(s => new { Id = s.id, Produto = s.descricao + " | " + s.tipoVariacao, Descricao = s.descricao }).ToList();
            ViewBag.Produtos = new SelectList(dropdown_produto, "Descricao", "Produto");

            //clientes

            RetornoTinyApi cliente = new RetornoTinyApi();
            var clientes = await cliente.ListarClientes(tokenTiny);
            var dropdown_cliente = clientes.retorno.contatos.Select(s => new { Id = s.contato.id, cliente = s.contato.nome, Nome =s.contato.nome}).ToList();
            ViewBag.Clientes = new SelectList(dropdown_cliente, "Nome", "Cliente" );
            int id_usurio_controller = id;
            return View();
        }
        [HttpPost]
        public ActionResult SalvarFoto(string dados_imagem, string produto_selecionado, string cliente_selecionado)
        {
            if (string.IsNullOrWhiteSpace(produto_selecionado) || string.IsNullOrWhiteSpace(cliente_selecionado))
            {
                TempData["mensagem"] = "Selecione o produto e o cliente";
            }
            else {
                byte[] vet_bytes = Convert.FromBase64String(dados_imagem);
                string caminho_diretorio = Server.MapPath("~/Fotos");

                if (!Directory.Exists(caminho_diretorio))
                {
                    Directory.CreateDirectory(caminho_diretorio);
                }
                DateTime data = DateTime.Now;
                string nome_arquivo = $"Produto_{produto_selecionado}_Cliente_{cliente_selecionado}.jpg";
                nome_arquivo = Remover_caracteres(nome_arquivo);
                string caminho_imagem = Path.Combine(caminho_diretorio, nome_arquivo);
                System.IO.File.WriteAllBytes(caminho_imagem, vet_bytes);

                

                return Json(new { sucesso = true, mensagem = "Foto salva com sucesso! \n" + nome_arquivo });
            }
           
               return Json(new { sucesso = false, mensagem = "Erro ao salvar a foto. Produto ou cliente não selecionados." });

        }
    
        private string Remover_caracteres(string caracter)
        {
            string padrao = "[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]";
            return Regex.Replace(caracter, padrao, "");
        }

      private string Salvar_dados(string produto, string cliente, int id, DateTime data)
        {
            try
            {
                using (Conexao conexao = new Conexao())
                {
                    string sql_insert = "insert into tb_fotos(id_usuario,nome_produto, nome_cliente,data_tirada) values (@id_u, @np, @nc,@dt)";

                    using(MySqlCommand comando = new MySqlCommand(sql_insert,conexao._conn)) 
                    { 
                        comando.Parameters.AddWithValue("@id_u", id);
                        comando.Parameters.AddWithValue("@np",produto);
                        comando.Parameters.AddWithValue("@nc",cliente);
                        comando.Parameters.AddWithValue("@dt",data);
                    }
                }
            }
            catch (Exception erro)
            {

                throw;
            }
            return mensagem;
        }
    }
   

}
