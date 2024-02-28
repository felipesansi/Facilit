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
       
        public ActionResult Index() //  essa é a tela de login sendo chamada
        {
            return View();
        }

        public ActionResult Perfis_usuario() // aqui  está sendo carregada com toda estrutura para mostrar todos os usuários do sistema
        {
            try
            {


                using (Conexao conexao = new Conexao()) //instanciando a classe conexao 
                {
                    string sql_select = "select * from tb_usuarios where excluido = false  "; //selecione todos os dados da tabela tb_usuarrios que estejam com excuido = 0

                    using (MySqlCommand comando = new MySqlCommand(sql_select, conexao._conn)) // instanciando a classe mysqlcommand com  o comando e string de conexao junto
                    {
                        MySqlDataReader leitura = comando.ExecuteReader(); //  instanciando a classe chamda leitura  que recebe o comando de executar o sql

                        if (Session["logado"] != null) // se o usuario estiver logado
                        {

                            if (leitura.HasRows) // se estiver linhas para ler
                            {
                                var listaUsuario = new List<Usuario>(); // criação de uma lista com as propiedade do modelo de usuario
                                while (leitura.Read()) // enquando estiver leitura
                                {
                                    var usuario = new Usuario // criar um obj das propiedades do modelo de usuario
                                    {
                                        Id = Convert.ToInt32(leitura["id"]), // o modelo de usuario na propiedade id recebe os dados já convertidos em int do banco no campo id  lido pela leitura
                                        Nome_completo = Convert.ToString(leitura["nome_completo"]), // o modelo de usuario na propiedade nome recebe os dados já convertidos em string do banco no campo nome  lido pela leitura
                                        Email = Convert.ToString(leitura["email"]), // como no anterior
                                        Nome_Usuario = Convert.ToString(leitura["nome_usuario"]), // como no anterior
                                        Criado = Convert.ToDateTime(leitura["criado"]), // como no anterior
                                        Alterado = Convert.ToDateTime(leitura["alterado"]) // como no anterior
                                    };
                                    listaUsuario.Add(usuario); //adiciondo todos esses dados da linha na lista e voltando enquanto estiver linhas para serem lidas

                                }
                                return View(listaUsuario); //passando a lista para view

                            }

                            else // se  não estiverem linha para ler 
                            {
                                return RedirectToAction("Index", "Webcan"); // retorne a  view index no controller webcan
                            }
                        }
                        else // se o usuário não estiver logado
                        {
                            return RedirectToAction("Index", "Usuario"); // retorne a  view index no controller usuario
                           
                        }
                    }

                }
            }
            catch (Exception erro)
            {
                TempData["Mensagem"] = "Ocorreu um erro" + erro; // se hover erro está mensagem  irá aparecer
                return RedirectToAction("Index", "Usuario");
            }
        }

        public ActionResult PesqusarUsuario(string nome)
        {
            try
            {
                if (Session["logado"] != null)
                {
                    using (Conexao conexao = new Conexao())
                    {
                        string str_pesquisa = "select * from tb_usuarios where excluido = false and nome_completo like @p_nome";

                        using (MySqlCommand comando = new MySqlCommand(str_pesquisa, conexao._conn))
                        {
                            comando.Parameters.AddWithValue("@p_nome", "%" + nome + "%");

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
                                        Alterado = Convert.ToDateTime(leitura["alterado"]),
                                        Senha_Usuario =  Convert.ToString(leitura["senha_usuario"])

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
                else
                {
                    return RedirectToAction("Index", "Usuario");
                }

            }
            catch (Exception erro )
            {

                TempData["Mensagem"] = "Ocorreu um erro" + erro;
                return RedirectToAction("Index", "Usuario");
            }  

        }
        public ActionResult atualizarUsuario(Usuario usuario)
        {
            try
            {
                if (Session["logado"] != null) //se tiver logado
                {
                    // se qualquer um dos campos estiverem vazios irá aparecer uma mensagem
                    if (string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Email) ||
                        string.IsNullOrWhiteSpace(usuario.Nome_Usuario) || string.IsNullOrWhiteSpace(usuario.Senha_Usuario))
                    {
                        TempData["Mensagem"] = "Atenção: Há algum campo vazio";
                        return RedirectToAction("Editar", "Usuario", new { id = usuario.Id });
                    }

                    // se os campos não estiverem vazios, mas o usuário digitar uma senha menor que 4 caracteres
                    else if (usuario.Nome_Usuario.Length < 4 || usuario.Senha_Usuario.Length < 4)
                    {
                        TempData["Mensagem"] = "Usuário ou senha com menos de 4 caracteres";

                        return RedirectToAction("Editar", "Usuario", new { id = usuario.Id });

                    }
                    else
                    { // se os campos não estiverem vazios e a senha forem maior que 3 atualize os dados

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
                                comando.Parameters.AddWithValue("@alterado", DateTime.Now); // pega a data atual do sistema e armazena no banco
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
            catch (Exception erro)
            {

                TempData["Mensagem"] = "Ocorreu um erro" + erro;
                return RedirectToAction("Index", "Usuario");
            }
        }

        public ActionResult Editar(int id)
        {
            try
            {

                if (Session["logado"] != null)
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
            catch (Exception erro)
            {


                TempData["Mensagem"] = "Ocorreu um erro" + erro;
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
            using (Conexao conexao = new Conexao())
            {
                using (MySqlCommand comando = new MySqlCommand(str_delete, conexao._conn))
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

                        if (Session["logado"] != null)
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

        public ActionResult NovaSenha(string token, Usuario usuario)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Mensagem"] = "Esse token não existe ";
                return RedirectToAction("RecuperarSenha", "Usuario");


            }
            bool verificado = VerificarToken(token);

            if (!verificado)
            {
                TempData["Mensagem"] = "O token passou do 1 Hora ";
                return RedirectToAction("RecuperarSenha", "Usuario");

            }
            return View("NovaSenha");
        }
        public ActionResult AtualizarSenha(Usuario usuario)
        {



            if (usuario.Senha_Usuario == usuario.Confirmar_Senha )
            {
                if (usuario.Senha_Usuario.Length >3 && usuario.Confirmar_Senha.Length >3  )
                {
                    string sql_atualizar = "update tb_usuarios set senha_usuario = @senha_usuario where email= @email";
                    using (Conexao conexao = new Conexao())
                    {
                        using (MySqlCommand comando = new MySqlCommand(sql_atualizar, conexao._conn))
                        {
                            if (EmailExistente(usuario))
                            {
                                comando.Parameters.AddWithValue("@email", usuario.Email);
                                comando.Parameters.AddWithValue("@senha_usuario", usuario.Senha_Usuario);
                                comando.ExecuteNonQuery();
                                TempData["Sucesso"] = "Sua senha foi atualizada com sucesso!\n\n Volte ao login";
                           

                            }
                            else
                            {
                                TempData["Mensagem"] = "O e-mail não está correto";
                            }


                        }
                    }
                }
                else
                {
                    TempData["Mensagem"] = "A senha deve ter mais de 3 caracteres";
                }
                

            }
            else
            {
                TempData["Mensagem"] = "As senhas não correspondem";
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
            DateTime data_expedicao = DateTime.Now.AddHours(1);

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
            try
            {
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
            }
            catch (Exception erro)
            {

                TempData["Mensagem"] = "Ocorreu um erro" + erro;
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
                            string corpo = "Prezado " + nome +  "," +
                                "\n\n nome de Usuário: " + nome_usuario +
                                "\n\nEspero que esteja bem." +
                                " \n\nEstamos entrando em contato porque foi solicitada uma recuperação de senha para a sua conta." +
                                " \n\nPor favor, siga o link abaixo para redefinir sua senha:  " + link_nova_senha +
                                "\n\nEste link tem a duração de  1  HORA.\b\b\n\n você não solicitou esta recuperação ou se precisar de assistência adicional, " +
                                "não hesite em nos contatar imediatamente. Agradecemos sua atenção e cooperação. \n\nAtenciosamente, Felipe F. " +
                                "\n\n Facilit";



                            string remetente = "facilit.site@gmail.com", Smtp = "smtp.gmail.com", senha_email = "dujr xsqy luri jzat";

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

                                e_mail.Subject = "Solicitação de Recuperação de Senha - Facilit";

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

