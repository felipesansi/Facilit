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
        public ActionResult Administrador() 
        {
            string sql = "select * from tb_usuarios";
            using(var conexao = new Conexao())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conexao._conn)) 
                {
                    var lista_usuarios = new List<Usuario>(); //  criação de uma lista de dados de todos os usuários
                    MySqlDataReader leitura = cmd.ExecuteReader();
                  
                    while(leitura.Read()) // enquanto tiver dados
                    {
                        var prop_usuario = new Usuario
                        {
                            Nome_completo = Convert.ToString(leitura["nome_completo"]),
                            Email = Convert.ToString(leitura["email"]),
                            Nome_Usuario = Convert.ToString(leitura["nome_usuario"]),
                            Senha_Usuario = Convert.ToString(leitura["senha_usuario"]),
                            Adm = Convert.ToBoolean(leitura["adm"])

                        };
                        lista_usuarios.Add(prop_usuario);
                    }


                }
            }
            return View();
        }
        
        public ActionResult VerificarLogin(Usuario class_usuario) 
        {
            using(var conexao = new Conexao())
            {
                string str_select = "select * from tb_usuarios where nome_usuario = @nome_usuario " +
                    "and senha_usuario = @senha "
                    ;

                using (MySqlCommand comando = new MySqlCommand(str_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nome_usuario", class_usuario.Nome_Usuario);

                    comando.Parameters.AddWithValue("@senha" ,class_usuario.Senha_Usuario);


                    MySqlDataReader leitura = comando.ExecuteReader();
                    leitura.Read();
                    if (leitura.HasRows)
                    {
                        bool adm = Convert.ToBoolean(leitura["adm"]);

                        if (adm)
                        {
                            return RedirectToAction("Administrador", "Usuario");
                        }
                        else
                        {
                            return RedirectToAction("Registro", "Usuario");
                        }
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
            string sql_insert = "insert into usuarios (nome_completo, email, nome_usuario, senha_usuario, adm) values " +
                "(@nc, @em, @nu, @su, @ad)";
            using (var conexao = new Conexao())
            {

                using (MySqlCommand comando = new MySqlCommand(sql_insert, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nc",usuario.Nome_completo);
                    comando.Parameters.AddWithValue("@em",usuario.Email);
                    comando.Parameters.AddWithValue("@nu",usuario.Nome_Usuario);
                    comando.Parameters.AddWithValue("@su",usuario.Senha_Usuario);
                    comando.Parameters.AddWithValue("@ad",usuario.Adm);

                    comando.ExecuteNonQuery();

                    return RedirectToAction("Administrador", "Usuario");
                }
                
            }
        }


    }

}