using Facilit.Models;
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
                        
                        return RedirectToAction("Index", "Webcan");
                    }
                }

            }
        }
        public ActionResult atualizarUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Email) ||
                string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Senha_Usuario))
            {
                TempData["Mensagem"] = "A algum campo vazio";
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

        public ActionResult Editar(int id)
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
                        ViewBag.excluido = true;
                        return RedirectToAction("Perfis_usuario", "Usuario");
                    }
                }
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
                        ViewBag.excluido = true;
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
                        return RedirectToAction("Registro", "Webcan");

                    }
                    else
                    {
                        TempData["Mensagem"] = "Usuário ou senha incorretos";
                        return RedirectToAction("Index", "Usuario");
                    }

                }
            }
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
                TempData["Mensagem"] = "A algum campo vazio";

                return RedirectToAction("Cadastro", "Usuario");
            }
            else if (existe == ExisteUsuario(usuario))
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
            Session["existe"] = false;
            string sql_select = "select * from tb_usuarios where nome_usuario = @nome_usuario";

            using (var conexao = new Conexao())
            {
                using (var comando = new MySqlCommand(sql_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nome_usuario", usuario.Nome_Usuario);
                    MySqlDataReader leitura = comando.ExecuteReader();

                    if (leitura.HasRows)
                    {
                        Session["existe"] = true;
                    }

                }
            }

            return existe;


        }


        public ActionResult EmailEnviado()
        {
            return View();
        }
        bool existe_email = false;
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
         
            string remetente = "facilit.site@gmail.com", senha_remetente = "trwn qsch rwze anar", stmp = "smtp.gmail.com";
            int port = 587;

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

                            SmtpClient client;
                            NetworkCredential login;
                            MailMessage msg;

                            try
                            {
                                login = new NetworkCredential(remetente,senha_remetente);
                                client = new SmtpClient(stmp);
                                client.Port = port;
                                client.EnableSsl = true;



                                
                                msg = new MailMessage();
                                msg.To.Add(usuario.Email);
                                msg.From = new MailAddress(remetente);
                                

                                msg.Subject = "Solicitação de Recuperação de senha - Facilit";
                                msg.Body = " Olá, " + nome +  "sua Senha : " + senha +  " \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n Este e-mail é automático.\nNão responda";


                                msg.IsBodyHtml = true;

                                msg.Priority = MailPriority.High;

                                client.Send(msg);

                                return RedirectToAction("EmailEnviado", "Usuario");
                                

                            }
                            catch (Exception ex)
                            { 
                                ViewBag.erro = "Ocorreu um erro ao enviar o e-mail: " + ex.Message;
                                
                            }

                        }
                    }
                }
            }

            return RedirectToAction("RecuperarSenha", "Usuario");
        }


    }
}
