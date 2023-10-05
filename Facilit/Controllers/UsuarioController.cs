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

                        };
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
                    "and senha_usuario = @senha and adm =true"
                    ;

                using (MySqlCommand comando = new MySqlCommand(str_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@nome_usuario", class_usuario.Nome_Usuario);

                    comando.Parameters.AddWithValue("@senha" ,class_usuario.Senha_Usuario);


                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        return RedirectToAction("Administrador");
                    }
                    else
                    {
                       
                        return RedirectToAction("Index");
                       
                    }

                }
            }
        }
        public ActionResult RecuperarSenha()
        {
            return View();  
        }
    }
}