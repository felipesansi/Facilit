using System;
using System.IO;
using System.Web.Mvc;

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
            string caminho_diretorio = Server.MapPath("~/Fotos");
            string caminho_imagem = Path.Combine(caminho_diretorio, "Produto_foto.jpg");

          
            if (!Directory.Exists(caminho_diretorio))
          
            {
                Directory.CreateDirectory(caminho_diretorio);
            }


            

            System.IO.File.WriteAllBytes(caminho_imagem, vet_bytes);
            return Json(new { sucesso = true, mensagem = "Foto salva com sucesso" });
        }

    }
}
