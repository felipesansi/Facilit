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
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Facilit.Controllers
{
    public class WebcanController : Controller
    {
        string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
        string mensagem;
        int usuario_id;
        public async Task<ActionResult> Registro()
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
                usuario_id = (int)Session["id_usuario"];
                string caminho_imagem = Path.Combine(caminho_diretorio, nome_arquivo);
                System.IO.File.WriteAllBytes(caminho_imagem, vet_bytes);

                Salvar_dados(produto_selecionado, cliente_selecionado, usuario_id, data);

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
                using ( var conexao = new Conexao())
                {
                    string sql_insert = "insert into tb_fotos(id_usuario,nome_produto,nome_cliente,data_tirada) values (@id_u, @np, @nc,@dt)";

                    using(MySqlCommand comando = new MySqlCommand(sql_insert,conexao._conn)) 
                    { 
                        comando.Parameters.AddWithValue("@id_u", id);
                        comando.Parameters.AddWithValue("@np",produto);
                        comando.Parameters.AddWithValue("@nc",cliente);
                        comando.Parameters.AddWithValue("@dt",data);
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception erro)
            {

              TempData["mensagem"] = "Ocorreu um erro: " + erro.Message;
            }
            return mensagem;
        }
        public ActionResult Gerador_pdf()
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string select_join = "SELECT tb_usuarios.nome_completo, tb_fotos.nome_produto, " +
                                         "tb_fotos.nome_cliente, tb_fotos.data_tirada, tb_fotos.id " +
                                         "FROM tb_usuarios JOIN tb_fotos " +
                                         "ON tb_usuarios.id = tb_fotos.id_usuario AND tb_usuarios.excluido = false";

                    using (MySqlCommand comando = new MySqlCommand(select_join, conexao._conn))
                    {
                        MySqlDataReader leitura = comando.ExecuteReader();

                        if (leitura.HasRows)
                        {
                            // Configuração do documento
                            Document documento = new Document();
                            MemoryStream ms = new MemoryStream();
                            PdfWriter escreve = PdfWriter.GetInstance(documento, ms);
                            
                            documento.Open();

                           
                            PdfPTable tabela = new PdfPTable(5); // instância da tabela com 5 colunas
                            Font fonte_noto = FontFactory.GetFont("Noto Sans",12,Font.BOLD);

                            float paddingPadrao = 5f;

                            PdfPCell coluna_funcionario = new PdfPCell(new Phrase("Nome do Funcionário",fonte_noto));
                            coluna_funcionario.BackgroundColor = BaseColor.LIGHT_GRAY;
                            coluna_funcionario.PaddingRight = paddingPadrao;
                            coluna_funcionario.PaddingLeft = paddingPadrao;
                            tabela.AddCell(coluna_funcionario);


                            PdfPCell coluna_Produto  = new PdfPCell(new Phrase("Nome do Produto", fonte_noto));
                            coluna_Produto.BackgroundColor = BaseColor.LIGHT_GRAY;
                            coluna_Produto.PaddingRight = paddingPadrao;
                            coluna_Produto.PaddingLeft = paddingPadrao;
                            tabela.AddCell(coluna_Produto);


                            PdfPCell coluna_cliente = new PdfPCell(new Phrase("Nome do Cliente", fonte_noto));
                            coluna_cliente.BackgroundColor = BaseColor.LIGHT_GRAY;
                            coluna_cliente.PaddingRight = paddingPadrao;
                            coluna_cliente.PaddingLeft = paddingPadrao;
                            tabela.AddCell(coluna_cliente);

                            PdfPCell coluna_data = new PdfPCell(new Phrase("Data de Emissão da Foto", fonte_noto));
                            coluna_data.BackgroundColor = BaseColor.LIGHT_GRAY;
                            tabela.AddCell(coluna_data);

                            PdfPCell coluna_id = new PdfPCell(new Phrase("Ordem das emissão das Fotos", fonte_noto));
                            coluna_id.BackgroundColor = BaseColor.LIGHT_GRAY;
                            tabela.AddCell(coluna_id);

                            while (leitura.Read())
                            {
                                tabela.AddCell(leitura["nome_completo"].ToString());
                                tabela.AddCell(leitura["nome_produto"].ToString());
                                tabela.AddCell(leitura["nome_cliente"].ToString());
                                tabela.AddCell(Convert.ToDateTime(leitura["data_tirada"]).ToString("dd/MM/yyyy HH:mm:ss"));
                                tabela.AddCell(Convert.ToInt16(leitura["id"]).ToString());

                            }

                            documento.Add(tabela);
                            documento.Close();

                            byte[] fileBytes = ms.ToArray();
                            return File(fileBytes, "application/pdf", "Dados Sistema Facilit.pdf");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["mensagem"] = "Ocorreu um erro ao gerar o PDF: " + ex.Message;
                return RedirectToAction("Registro"); 
            }

            
            TempData["mensagem"] = "Não há dados para gerar o PDF.";
            return RedirectToAction("Registro");
        }
    }
   

}
