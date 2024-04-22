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

        [Required(ErrorMessage ="Preencha o campo Nome")]
        public string Nome_completo { get; set; }
       
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha o campo Nome de usuário")]
        public string Nome_Usuario { get; set; }

        [Required(ErrorMessage = "Preencha o campo Senha")]
        public string Senha_Usuario { get; set; }
        [Required(ErrorMessage = "Preencha o campo Confirmar Senha")]
        public string Confirmar_Senha { get; set; }
        public bool Excluido { get; set; }

        public DateTime Criado { get; set; }
         public DateTime Alterado { get; set; }




    }
  
    

}