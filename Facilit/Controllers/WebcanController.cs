using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace Facilit.Controllers
{
    public class WebcanController : Controller
    {
       
        public ActionResult Registro()
        {
            return View();

        }
        [HttpPost]
        public ActionResult SalvarFoto(string dados_imagem)
        {
            byte[] vet_bytes = Convert.FromBase64String(dados_imagem);

            string caminho_imagem = Server.MapPath("~\\Facilit\\Fotos\\Produto_foto.jpg");


            System.IO.File.WriteAllBytes(caminho_imagem, vet_bytes);

            return Json(new { sucesso = true, mensagem = "Foto salva com sucesso" });
        }

    }
}