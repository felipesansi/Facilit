using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class Conexao : IDisposable
    {
      public MySqlConnection _conn;

        private readonly string _server = "server-facilit.mysql.database.azure.com";
        private readonly string _port = "3306";
        private readonly string _database = "bd_facilit";
        private readonly string _uid = "Felipe";
        private readonly string _pwd = "banco123@";

        public Conexao()
        {
            Conectar();
        }

        public void Conectar()
        {
            string str_conexao = "Server =" + _server;
            str_conexao += "; Port =" + _port;
            str_conexao += "; Database =" + _database;
            str_conexao += "; Uid =" + _uid;
            str_conexao += "; Pwd =" + _pwd;

            _conn = new MySqlConnection(str_conexao);
            
                try
                {
                    _conn.Open();

                }
                catch (Exception ex)
                {

                    throw ex;

                }

            
        }

      public void Dispose()
        {
            _conn.Clone();
            _conn.Dispose();
        }
    }
    
}