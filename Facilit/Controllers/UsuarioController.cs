﻿using Facilit.Models;
using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Renci.SshNet;
using System.Net;

namespace Facilit.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Perfis_usuario()
        {

            using (Conexao conexao = new Conexao())
            {
                string sql_select = "select * from tb_usuarios where excluido = false  ";

                using (MySqlCommand comando = new MySqlCommand(sql_select, conexao._conn))
                {
                    MySqlDataReader leitura = comando.ExecuteReader();

                    if (Session["logado"] != null)
                    {

                        if (leitura.HasRows)
                        {
                            var listaUsuario = new List<Usuario>();
                            while (leitura.Read())
                            {
                                var usuario = new Usuario
                                {
                                    Id = Convert.ToInt32(leitura["id"]),
                                    Nome_completo = Convert.ToString(leitura["nome_completo"]),
                                    Email = Convert.ToString(leitura["email"]),
                                    Nome_Usuario = Convert.ToString(leitura["nome_usuario"]),
                                    Criado = Convert.ToDateTime(leitura["criado"]),
                                    Alterado = Convert.ToDateTime(leitura["alterado"])
                                };
                                listaUsuario.Add(usuario);

                            }
                            return View(listaUsuario);

                        }

                        else
                        {
                            return RedirectToAction("Index", "Usuario");
                        }
                    }
                    else
                    {

                        return RedirectToAction("Index", "Webcan");
                    }
                }

            }
        }

        public ActionResult PesqusarUsuario(string nome)
        {
            using(Conexao conexao = new Conexao())
            {
                string str_pesquisa = "select * from tb_usuarios where excluido = false and nome_completo like @p_nome";

                using (MySqlCommand comando = new MySqlCommand(str_pesquisa, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@p_nome", "%" + nome + "%");

                    MySqlDataReader leitura = comando.ExecuteReader();
                    if (true)
                    {

                    }
                    if (leitura.HasRows)
                    {
                      var listaUsuario = new List<Usuario>();
                        while(leitura.Read())
                        {
                            var usuario = new Usuario
                            {
                                Id = Convert.ToInt32(leitura["id"]),
                                Nome_completo = Convert.ToString(leitura["nome_completo"]),
                                Email = Convert.ToString(leitura["email"]),
                                Nome_Usuario = Convert.ToString(leitura["nome_usuario"]),
                                Criado = Convert.ToDateTime(leitura["criado"]),
                                Alterado = Convert.ToDateTime(leitura["alterado"])
                            };
                            listaUsuario.Add(usuario);

                        }
                        return View("Perfis_usuario", listaUsuario);

                    }
                    else
                    {
                        TempData["Mensagem"] = "Atenção: Usuário não encontrado";
                        return View("Perfis_usuario", "Usuario");
                    }

                }
                
            }

        }
        public ActionResult atualizarUsuario(Usuario usuario)
        {
            if (Session["logado"]!= null)
            {

                if (string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Email) ||
                    string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Senha_Usuario))
                {
                    TempData["Mensagem"] = "Atenção: Há algum campo vazio";
                    return RedirectToAction("Editar", "Usuario", new { id = usuario.Id });
                }


                else if (usuario.Nome_Usuario.Length < 4 || usuario.Senha_Usuario.Length < 4)
                {
                    TempData["Mensagem"] = "Usuário ou senha com menos de 4 caracteres";

                    return RedirectToAction("Editar", "Usuario", new { id = usuario.Id });

                }
                else
                {

                    string sql_insert = "update tb_usuarios set " +
                            "nome_completo = @nc, email = @em , nome_usuario = @nu, senha_usuario = @su, alterado = @alterado where id = @id";

                    using (var conexao = new Conexao())
                    {

                        using (MySqlCommand comando = new MySqlCommand(sql_insert, conexao._conn))
                        {
                            comando.Parameters.AddWithValue("@nc", usuario.Nome_completo);
                            comando.Parameters.AddWithValue("@em", usuario.Email);
                            comando.Parameters.AddWithValue("@nu", usuario.Nome_Usuario);
                            comando.Parameters.AddWithValue("@su", usuario.Senha_Usuario);
                            comando.Parameters.AddWithValue("@alterado", DateTime.Now);
                            comando.Parameters.AddWithValue("@id", usuario.Id);

                            comando.ExecuteNonQuery();

                            return RedirectToAction("Index", "Usuario");
                        }


                    }
                }

            }
            else
            {
                return RedirectToAction("Index", "Usuario");
            }
        }

        public ActionResult Editar(int id)
        {
            if (Session["logado"]!=null)
            {
                string str_editar = "select * from tb_usuarios where id = @id";

                using (Conexao conexao = new Conexao())
                {

                    using (MySqlCommand comando = new MySqlCommand(str_editar, conexao._conn))
                    {


                        comando.Parameters.AddWithValue("@id", id);

                        MySqlDataReader leitura = comando.ExecuteReader();
                        leitura.Read();
                        if (leitura.HasRows)
                        {

                            var usuario = new Usuario
                            {
                                Id = Convert.ToInt32(leitura["id"]),
                                Nome_completo = Convert.ToString(leitura["nome_completo"]),
                                Email = Convert.ToString(leitura["email"]),
                                Nome_Usuario = Convert.ToString(leitura["nome_usuario"]),
                                Senha_Usuario = Convert.ToString(leitura["senha_usuario"]),
                            };
                            return View(usuario);

                        }
                        else
                        {

                            return RedirectToAction("Perfis_usuario", "Usuario");
                        }
                    }
                }



            }
            else
            {
                return RedirectToAction("Index", "Usuario");
            }
        }


        public ActionResult Excluir(int id)
        {
            string str_editar = "select * from tb_usuarios where id = @id";

            using (Conexao conexao = new Conexao())
            {

                using (MySqlCommand comando = new MySqlCommand(str_editar, conexao._conn))
                {


                    comando.Parameters.AddWithValue("@id", id);

                    MySqlDataReader leitura = comando.ExecuteReader();
                    leitura.Read();
                    if (leitura.HasRows)
                    {

                        var usuario = new Usuario
                        {
                            Id = Convert.ToInt32(leitura["id"]),
                            Nome_completo = Convert.ToString(leitura["nome_completo"]),
                            Email = Convert.ToString(leitura["email"]),
                            Nome_Usuario = Convert.ToString(leitura["nome_usuario"]),
                            Senha_Usuario = Convert.ToString(leitura["senha_usuario"]),
                        };
                        return View(usuario);

                    }
                    else
                    {
                     
                        return RedirectToAction("Perfis_usuario", "Usuario");
                    }
                }
            }



        }



        public ActionResult Softdelete(Usuario usuario)
        {
            string str_delete = "update tb_usuarios set excluido = true, alterado = @alterado where id = @id";
            using(Conexao conexao = new Conexao())
            {
                using(MySqlCommand comando = new MySqlCommand(str_delete,conexao._conn))
                { 
                    comando.Parameters.AddWithValue("id", usuario.Id);    
                   
                    comando.Parameters.AddWithValue("@alterado", DateTime.Now);
                   
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Perfis_usuario", "Usuario");
                }

            }
        }
        
        public ActionResult Cadastro()
        {
            return View();
        }

        bool existe = false;
       
        [HttpPost]
        public ActionResult VerificarLogin(Usuario class_usuario)
        {
            using (var conexao = new Conexao())
            {
                string str_select = "select * from tb_usuarios where nome_usuario = @nome_usuario " +
                    "and senha_usuario = @senha and excluido = false";

                using (MySqlCommand comando = new MySqlCommand(str_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nome_usuario", class_usuario.Nome_Usuario);

                    comando.Parameters.AddWithValue("@senha", class_usuario.Senha_Usuario);


                    MySqlDataReader leitura = comando.ExecuteReader();
                    leitura.Read();
                    if (leitura.HasRows)
                    {
                        Session["logado"] = true;
                       
                        if (Session["logado"]!= null)
                        {
                               return RedirectToAction("Registro", "Webcan");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Usuario");
                        }

                    
                  
                    }
                    else
                    {
                        Session["logado"] = false;
                        TempData["Mensagem"] = "Usuário ou senha incorretos";
                        return RedirectToAction("Index", "Usuario");
                    }

                }
            }
        }

        public ActionResult NovaSenha( string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("RecuperarSenha", "Usuario");
            }
            bool verificado = VerificarToken(token);

            if (!verificado)
            {
                return RedirectToAction("RecuperarSenha", "Usuario");
            }
            return View("NovaSenha");
        }
        public ActionResult RecuperarSenha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NovoUsuario(Usuario usuario)
        {

            if (string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Email) ||
      string.IsNullOrWhiteSpace(usuario.Nome_completo) || string.IsNullOrEmpty(usuario.Senha_Usuario))
            {
                // Define a mensagem
                TempData["Mensagem"] = "Há algum campo vazio";

                return RedirectToAction("Cadastro", "Usuario");
            }
            else if ( ExisteUsuario(usuario))
            {
                TempData["Mensagem"] = "Já existe uma conta com este usuário";
                return RedirectToAction("Cadastro", "Usuario");
            }

            else if (usuario.Nome_Usuario.Length < 4 || usuario.Senha_Usuario.Length < 4)
            {
                TempData["Mensagem"] = "Usuário ou senha com menos de 4 caracteres";

                return RedirectToAction("Cadastro", "Usuario");
            }

            else
            {

                string sql_insert = "insert into tb_usuarios (nome_completo, email, nome_usuario, senha_usuario, criado) values " +
"(@nc, @em, @nu, @su, @criado)";

                using (var conexao = new Conexao())
                {

                    using (MySqlCommand comando = new MySqlCommand(sql_insert, conexao._conn))
                    {
                        comando.Parameters.AddWithValue("@nc", usuario.Nome_completo);
                        comando.Parameters.AddWithValue("@em", usuario.Email);
                        comando.Parameters.AddWithValue("@nu", usuario.Nome_Usuario);
                        comando.Parameters.AddWithValue("@su", usuario.Senha_Usuario);
                        comando.Parameters.AddWithValue("@criado", DateTime.Now);


                        comando.ExecuteNonQuery();

                        return RedirectToAction("Index", "Usuario");
                    }


                }
            }

        }

        [HttpPost]
        private bool ExisteUsuario(Usuario usuario)
        {
            existe = false;
            string sql_select = "select * from tb_usuarios where nome_usuario = @nome_usuario";

            using (var conexao = new Conexao())
            {
                using (var comando = new MySqlCommand(sql_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nome_usuario", usuario.Nome_Usuario);
                    MySqlDataReader leitura = comando.ExecuteReader();

                    if (leitura.HasRows)
                    {
                        existe = true;
                    }

                }
            }

            return existe;


        }


        public ActionResult EmailEnviado()
        {
            string email = TempData["Email"] as string;
            return View((object)email);
        }
        public static string GerarTokenUnico()
        {
            string token = Guid.NewGuid().ToString();

            return token;
        }
        private void SalvarToken(string email , string token)
        {
            DateTime data_expedicao = DateTime.Now.AddMinutes(5);

            string sql_updade = "update tb_usuarios set token = @token, token_expiracao = @expiracao where email = @email";
            using (Conexao conexao = new Conexao())
            {
                using (MySqlCommand comando = new MySqlCommand(sql_updade, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@token", token);
                    comando.Parameters.AddWithValue("@expiracao", data_expedicao);
                    comando.Parameters.AddWithValue("@email", email);
                    comando.ExecuteNonQuery();
                }
            }

        }
    
        

        private bool EmailExistente(Usuario usuario)
        {
            bool existe_email = false;
            using (var conexao = new Conexao())
            {
                string sql = "select * from tb_usuarios where  email = @email";

                using (var comando = new MySqlCommand(sql, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@email", usuario.Email);

                    MySqlDataReader leitura = comando.ExecuteReader();
                    if (leitura.HasRows)
                    {
                        existe_email = true;
                    }
                }
            }
            return existe_email;
        }

    

        public ActionResult EnviarEmail(Usuario usuario)
        {
     

            if (EmailExistente(usuario))
            {
                using (var conexao = new Conexao())
                {
                    string sql = "select * from tb_usuarios  where email = @email and excluido = false";
                    using (var comando = new MySqlCommand(sql, conexao._conn))
                    {
                        comando.Parameters.AddWithValue("@email", usuario.Email);

                        MySqlDataReader leitura = comando.ExecuteReader();
                        if (leitura.Read())
                        {
                            string nome = leitura.GetString(1);
                            string email = leitura.GetString(2);
                            string nome_usuario = leitura.GetString(3);
                            string senha = leitura.GetString(4);

                            // config para enviar email 


                            string token = GerarTokenUnico();

                            SalvarToken(email, token);
                            
                            string link_nova_senha = Url.Action("NovaSenha", "Usuario", new { token = token }, Request.Url.Scheme);
                            string corpo = "Prezado " + nome + "," +
                                "\n\nEspero que esteja bem." +
                                " \n\nEstamos entrando em contato porque foi solicitada uma recuperação de senha para a sua conta." +
                                " \n\nPor favor, siga o link abaixo para redefinir sua senha:  " + link_nova_senha +
                                "\n\nEste link tem a duração de  5 minutos.\b\b\n\n você não solicitou esta recuperação ou se precisar de assistência adicional, " +
                                "não hesite em nos contatar imediatamente. Agradecemos sua atenção e cooperação. \n\nAtenciosamente, Felipe F. " +
                                "\n\n Facilit";



                            string remetente = "facilit.site@outlook.com", Smtp = "smtp-mail.outlook.com", senha_email = "FelipeMatheus";

                            try
                            {
                                SmtpClient client = new SmtpClient(Smtp);

                                client.Port = 587;
                                
                                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                
                                client.UseDefaultCredentials = false;
                                
                                NetworkCredential credential = new NetworkCredential(remetente,senha_email);
                                client.EnableSsl = true;
                                client.Credentials = credential;
                                client.Timeout = 10000;
                                MailMessage e_mail = new MailMessage();
                                
                                e_mail.From = new MailAddress(remetente);
                                TempData["email"] = usuario.Email;
                                e_mail.To.Add(usuario.Email);

                                e_mail.Subject = "Solicitação de Recuperação de senha - Facilit";

                                e_mail.Body =corpo;
                              
                                e_mail.IsBodyHtml = false;

                                client.Send(e_mail);
                                 return RedirectToAction("EmailEnviado", "Usuario");
                            }
                            catch (Exception ex)
                            {

                                TempData["Mensagem"] = "Ocorreu um erro: " + ex;
                                return RedirectToAction("RecuperarSenha","Usuario");
                            }

                        }
                    }
                }
            }
            else
            {
                TempData["Mensagem"] = "O e-mail informado não está vinculdo há nenhuma conta ";
                return RedirectToAction("RecuperarSenha", "Usuario");
            }

            return RedirectToAction("RecuperarSenha", "Usuario");
        }
      
        
        private bool VerificarToken(string token)
        {
            bool verificado = false;
           
            DateTime data_atual = DateTime.Now;

            string sql_contar = "select count(*) from tb_usuarios where token = @token and token_expiracao > @dataAtual";

            try
            {
                using(Conexao conexao = new Conexao())
                {
                    using(MySqlCommand comando = new MySqlCommand(sql_contar, conexao._conn))
                    { 
                        comando.Parameters.AddWithValue("@token", token);
                        comando.Parameters.AddWithValue("@dataAtual", data_atual);

                         int contador = Convert.ToInt32(comando.ExecuteScalar());
                        verificado = contador > 0;
                    }
               
                    
                }
                
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Ocorreu um erro: " + ex;


                
            }
            return verificado;
        }

        

    }
}

