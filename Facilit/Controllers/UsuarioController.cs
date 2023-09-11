using Facilit.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

        public ActionResult VerificarLogin(Usuario class_usuario) 
        {
            using(var conexao = new Conexao())
            {
                string str_select = "SELECT * FROM tb_usuarios WHERE nome_usuario = @user OR nome_competo = @nome AND senha_usuario = @password";

                using (MySqlCommand comando = new MySqlCommand(str_select, conexao._conn))
                {
                    comando.Parameters.AddWithValue("@user", class_usuario.Nome_Usuario);

                    comando.Parameters.AddWithValue("@nome", class_usuario.Nome_completo);
                    comando.Parameters.AddWithValue("@password" ,class_usuario.Senha_Usuario);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        return RedirectToAction("Menu");
                    }
                    else
                    {
                       
                        return RedirectToAction("Index");
                    }

                }
            }
        }
    }
}