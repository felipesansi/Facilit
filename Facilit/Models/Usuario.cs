using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facilit.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome_completo { get; set; }
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        public string Nome_Usuario { get; set; }
        public string Senha_Usuario { get; set; }
        public bool Excluido { get; set; }




    }
  
    

}