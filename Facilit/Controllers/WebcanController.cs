using Facilit.Models;
using Facilit.Models.ClienteTiny;
using Facilit.Servicos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Facilit.Controllers
{
    public class WebcanController : Controller
    {
        string tokenTiny = "02011b49e5399d62d999007a8952642c85cca50bc310b49fdd6c3674fdff4b2a";
        string mensagem;
        int usuario_id;
        static string[] escopo = { DriveService.Scope.DriveFile };
        static string nomeApp = "facilit";

        public async Task Verificar_produtos()
        {
            using (var conexao = new Conexao())
            {
                string select_produto = "select id, codigo_tiny_produto, descricao, unidade, data_atualizacao_produto from tb_produtos limit 100";
                using (MySqlCommand comando = new MySqlCommand(select_produto, conexao._conn))
                {
                    MySqlDataReader leitura = comando.ExecuteReader();
                    if (leitura.HasRows)
                    {
                        var listaProdutos = new List<ProdutoTiny.Produts>();
                        try
                        {
                            while (leitura.Read())
                            {
                                var produts = new ProdutoTiny.Produts()
                                {
                                    id = Convert.ToInt32(leitura["id"]),
                                    codigo_tiny = Convert.ToInt32(leitura["codigo_tiny_produto"]),
                                    descricao = Convert.ToString(leitura["descricao"]),
                                    unidade = Convert.ToString(leitura["unidade"]),
                                    data_atualizacao = Convert.ToDateTime(leitura["data_atualizacao_produto"]),
                                };
                                listaProdutos.Add(produts);
                            }

                            ViewData["ListaProdutos"] = new SelectList(listaProdutos, "descricao", "descricao");
                        }
                        catch (Exception ex)
                        {
                            var erro = ex.Message;
                        }
                    }
                    else
                    {
                        TempData["Mensagem"] = "Carregando Dados dos Produtos AGUARDE ATÉ FINALIZAR";
                        RetornoTinyApi produto = new RetornoTinyApi();
                        var produtos = await produto.ListarProdutos(tokenTiny);
                        var dropdown_produto = produtos.retorno.produtos
                            .Select(s => new
                            {
                                Id = s.id,
                                Produto = s.descricao + " | " + s.tipoVariacao,
                                Descricao = s.descricao
                            })
                            .Take(100)
                            .ToList();
                        ViewData["ListaProdutos"] = new SelectList(dropdown_produto, "Descricao", "Produto");
                    }
                }
            }
        }

        public async Task Verificar_clientes()
        {
            string sql_select_clientes = "select codigo_tiny_cliente,nome,data_atualizacao_cliente from tb_clientes limit 500";
            using (var conexao = new Conexao())
            {
                using (MySqlCommand comando = new MySqlCommand(sql_select_clientes, conexao._conn))
                {
                    MySqlDataReader leitura = comando.ExecuteReader();

                    if (leitura.HasRows)
                    {
                        var lista_cliente = new List<Client>();
                        while (leitura.Read())
                        {
                            var client = new Client
                            {
                                codigo_tiny_cliente = Convert.ToInt32(leitura["codigo_tiny_cliente"]),
                                nome = Convert.ToString(leitura["nome"]),
                                data_atualizacao_cliente = Convert.ToDateTime(leitura["data_atualizacao_cliente"])
                            };
                            lista_cliente.Add(client);
                        }

                        ViewData["listarClientes"] = new SelectList(lista_cliente, "Nome", "Nome");
                    }
                    else
                    {
                        TempData["Mensagem"] = "Carregando Dados dos Clientes AGUARDE ATÉ FINALIZAR";
                        RetornoTinyApi cliente = new RetornoTinyApi();

                        var clientes = await cliente.ListarClientes(tokenTiny);
                        var dropdown_cliente = clientes.retorno.contatos
                            .Select(s => new
                            {
                                Id = s.contato.id,
                                Nome = s.contato.nome
                            })
                          .Take(500)
                          .ToList();
                        ViewData["listarClientes"] = new SelectList(dropdown_cliente, "Nome", "Nome");
                    }
                }
            }
        }

        public async Task<ActionResult> Registro()
        {
            if (Session["logado"] != null)
            {
                await Verificar_produtos();
                await Verificar_clientes();
                ViewBag.MostrarBotoes = false;
                ViewBag.MostrarContato = true;
            }
            else
            {
                RedirectToAction("Index", "usuario");
            }

            return View();
        }

     
        
            [HttpPost]

        public ActionResult SalvarFoto(string dados_imagem, string produto_selecionado, string cliente_selecionado)
        {
            if (string.IsNullOrWhiteSpace(produto_selecionado) || string.IsNullOrWhiteSpace(cliente_selecionado))
            {
                TempData["mensagem"] = "Selecione o produto e o cliente";
            }
            else
            {
                byte[] vet_bytes = Convert.FromBase64String(dados_imagem);

                
                var chave_secreta_cliente = new ClientSecrets
                {
                    ClientId = "522323830839-ipd9htma7tcogfgjvna23kepp87tql30.apps.googleusercontent.com",
                    ClientSecret = "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"
                };

                UserCredential credencial;
                using (var stream = new MemoryStream())
                {
                    var tokenResposta = new TokenResponse
                    {
                        RefreshToken = "1//04Od3Akb3yYrVCgYIARAAGAQSNwF-L9IrgII0Q6XgFGl-_Sz5THPZX039VaRgUjq6ISq6kut3h01lHeEeHJ8BxxxVPmn3qFET_Lg"
                    };

                    credencial = new UserCredential(new GoogleAuthorizationCodeFlow(
                        new GoogleAuthorizationCodeFlow.Initializer
                        {
                            ClientSecrets = chave_secreta_cliente,
                            Scopes = new[] { DriveService.Scope.Drive },
                        }), "user", tokenResposta);
                }

                var servico = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credencial,
                    ApplicationName = "facilit",
                });

      
                DateTime data = DateTime.Now;
                string nome_arquivo = $"Produto_{produto_selecionado}_Cliente_{cliente_selecionado}_{data.ToString("yyyyMMddHHmmss")}.jpg";
                nome_arquivo = Remover_caracteres(nome_arquivo);

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = nome_arquivo
                };

                FilesResource.CreateMediaUpload requisicao;
                using (var stream = new MemoryStream(vet_bytes))
                {
                    requisicao = servico.Files.Create(fileMetadata, stream, "image/jpeg");
                    requisicao.Fields = "id";
                    requisicao.Upload();
                }

                var arquivo = requisicao.ResponseBody;

                Salvar_dados(produto_selecionado, cliente_selecionado, usuario_id, data);

                return Json(new { sucesso = true, mensagem = "Foto salva com sucesso! \n Veja sua conta no Google Drive \n" + nome_arquivo });
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
                using (var conexao = new Conexao())
                {
                    string sql_insert = "insert into tb_fotos(id_usuario,nome_produto,nome_cliente,data_tirada) values (@id_u, @np, @nc,@dt)";
                    using (MySqlCommand comando = new MySqlCommand(sql_insert, conexao._conn))
                    {
                        comando.Parameters.AddWithValue("@id_u", id);
                        comando.Parameters.AddWithValue("@np", produto);
                        comando.Parameters.AddWithValue("@nc", cliente);
                        comando.Parameters.AddWithValue("@dt", data);
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception erro)
            {
                TempData["mensagem"] = "Ocorreu um erro: " + erro.Message;
            }
            return "Dados salvos com sucesso";
        }
        public ActionResult Gerador_pdf()
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string countQuery = "SELECT COUNT(tb_fotos.id) AS total_fotos " +
                                        "FROM tb_usuarios " +
                                        "JOIN tb_fotos ON tb_usuarios.id = tb_fotos.id_usuario AND tb_usuarios.excluido = false " +
                                        "WHERE tb_fotos.data_tirada >= DATE_SUB(CURDATE(), INTERVAL 10 DAY)";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, conexao._conn);
                    int totalFotos = Convert.ToInt32(countCommand.ExecuteScalar());

                    string select_join = "SELECT  tb_usuarios.id, tb_usuarios.nome_completo, tb_fotos.nome_produto, tb_fotos.nome_cliente," +
                        " tb_fotos.data_tirada FROM tb_usuarios JOIN tb_fotos ON tb_usuarios.id = tb_fotos.id_usuario AND tb_usuarios.excluido = false WHERE tb_fotos.data_tirada >= DATE_SUB(CURDATE(), INTERVAL 10 DAY)";

                    using (MySqlCommand comando = new MySqlCommand(select_join, conexao._conn))
                    {
                        using (MySqlDataReader leitura = comando.ExecuteReader())
                        {
                            if (leitura.HasRows)
                            {
                                Document documento = new Document(PageSize.A4);
                                MemoryStream ms = new MemoryStream();
                                PdfWriter escreve = PdfWriter.GetInstance(documento, ms);

                                documento.Open();

                                PdfPTable tabela = new PdfPTable(5);
                                float[] largura_das_colunas = new float[] { 15f, 70f, 60f, 60f, 70f };
                                tabela.SetTotalWidth(largura_das_colunas);
                                Font fonte = FontFactory.GetFont("HELVETICA", 12);

                                PdfPCell Titulo = new PdfPCell(new Phrase("Tabela Sistema Facilit", fonte));
                                Titulo.Colspan = 5;
                                Titulo.HorizontalAlignment = Element.ALIGN_CENTER;
                                Titulo.Phrase.Font.Size = 18;
                                Titulo.Border = PdfPCell.NO_BORDER;
                                Titulo.PaddingBottom = 25f;
                                tabela.AddCell(Titulo);

                                PdfPCell coluna_id = new PdfPCell(new Phrase("Id", fonte));
                                coluna_id.BackgroundColor = BaseColor.LIGHT_GRAY;
                                coluna_id.PaddingLeft = 5;
                                tabela.AddCell(coluna_id);

                                PdfPCell coluna_funcionario = new PdfPCell(new Phrase("Funcionário", fonte));
                                coluna_funcionario.BackgroundColor = BaseColor.LIGHT_GRAY;
                                tabela.AddCell(coluna_funcionario);

                                PdfPCell coluna_Produto = new PdfPCell(new Phrase("Produto", fonte));
                                coluna_Produto.BackgroundColor = BaseColor.LIGHT_GRAY;
                                coluna_Produto.PaddingLeft = 10;
                                tabela.AddCell(coluna_Produto);

                                PdfPCell coluna_cliente = new PdfPCell(new Phrase("Cliente", fonte));
                                coluna_cliente.BackgroundColor = BaseColor.LIGHT_GRAY;
                                coluna_cliente.PaddingLeft = 10;
                                tabela.AddCell(coluna_cliente);

                                PdfPCell coluna_data = new PdfPCell(new Phrase("Data da Foto", fonte));
                                coluna_data.BackgroundColor = BaseColor.LIGHT_GRAY;
                                tabela.AddCell(coluna_data);

                                while (leitura.Read())
                                {
                                    tabela.AddCell(Convert.ToInt16(leitura["id"]).ToString());
                                    tabela.AddCell(leitura["nome_completo"].ToString());
                                    tabela.AddCell(leitura["nome_produto"].ToString());
                                    tabela.AddCell(leitura["nome_cliente"].ToString());
                                    tabela.AddCell(Convert.ToDateTime(leitura["data_tirada"]).ToString("dd/MM/yyyy HH:mm:ss"));
                                }

                                documento.Add(tabela);

                                Paragraph contadorParagrafo = new Paragraph($"Total de fotos tiradas nos últimos 10 dias: {totalFotos}", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
                                contadorParagrafo.PaddingTop = 10;
                                contadorParagrafo.Alignment = Element.ALIGN_CENTER;
                                documento.Add(contadorParagrafo);

                                string escrito = "Esses dados sofrem alterações a cada 10 dias";
                                Paragraph paragrafo = new Paragraph(escrito, new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.ITALIC));
                                paragrafo.PaddingTop = 10;
                                paragrafo.Alignment = Element.ALIGN_CENTER;
                                documento.Add(paragrafo);

                                documento.Close();

                                byte[] fileBytes = ms.ToArray();
                                return File(fileBytes, "application/pdf", "Dados Sistema Facilit.pdf");
                            }
                        }
                    }
                }

                TempData["mensagem"] = "Não há dados para gerar o PDF.";
                return RedirectToAction("Registro");
            }
            catch (Exception ex)
            {
                TempData["mensagem"] = "Ocorreu um erro ao gerar o PDF: " + ex.Message;
                return RedirectToAction("Registro");
            }
        }


    }


}