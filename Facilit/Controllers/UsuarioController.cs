using Facilit.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facilit.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();

        }


        public ActionResult VerificarLogin(Usuario class_usuario)
        {
            using (var conexao = new Conexao())
            {
                string str_select = "select * from tb_usuarios where nome_usuario = @nome_usuario " +
                    "and senha_usuario = @senha";

                using (MySqlCommand comando = new MySqlCommand(str_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nome_usuario", class_usuario.Nome_Usuario);

                    comando.Parameters.AddWithValue("@senha", class_usuario.Senha_Usuario);


                    MySqlDataReader leitura = comando.ExecuteReader();
                    leitura.Read();
                    if (leitura.HasRows)
                    {
                        return RedirectToAction("Registro", "Usuario");

                    }
                    else
                    {
                        return RedirectToAction("Index", "Usuario");
                    }

                }
            }
        }
        public ActionResult RecuperarSenha()
        {
            return View();
        }
        public ActionResult NovoUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Email) ||
                string.IsNullOrWhiteSpace(usuario.Nome_Usuario))
            {

                ModelState.AddModelError("", "Preencha todos os campos");
                return RedirectToAction("Cadastro", "Usuario");


            } 

            else if(existe = ExisteUsuario(usuario))
            {
                ModelState.AddModelError("", "Nome de usuário já existe. Escolha outro nome de usuário.");
                return RedirectToAction("Cadastro", "Usuario");
            }

           

            else
            {
                string sql_insert = "insert into tb_usuarios (nome_completo, email, nome_usuario, senha_usuario) values " +
                    "(@nc, @em, @nu, @su )";
                using (var conexao = new Conexao())
                {

                    using (MySqlCommand comando = new MySqlCommand(sql_insert, conexao._conn))
                    {
                        comando.Parameters.AddWithValue("@nc", usuario.Nome_completo);
                        comando.Parameters.AddWithValue("@em", usuario.Email);
                        comando.Parameters.AddWithValue("@nu", usuario.Nome_Usuario);
                        comando.Parameters.AddWithValue("@su", usuario.Senha_Usuario);


                        comando.ExecuteNonQuery();

                        return RedirectToAction("Index", "Usuario");
                    }

                }
            }

        } 
        bool existe = false;
        private bool ExisteUsuario(Usuario usuario)
        {
            string sql_select = "select * from tb_usuarios where nome_usuario = @nome_usuario";

            using(var conexao = new Conexao())
            {
                using (var comando = new MySqlCommand( sql_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nome_usuario", usuario.Nome_Usuario);
                    MySqlDataReader leitura = comando.ExecuteReader();

                    if ( leitura.HasRows ) 
                    {
                        existe = true;
                    }

                }
            }

            return existe;

           
        }
        ActionResult EmailEnviado()
        {
             return View(); 
        }
        private ActionResult EnviarEmail(string destinatario)
        {
            string remetente = "facilit.site@gmail.com", senha_remetente = "FelipeMatheus", stmp = "smtp.gmail.com";
            int port = 587;
            return RedirectToAction(" EmailEnviado", "Usuario");
        }


    }

}