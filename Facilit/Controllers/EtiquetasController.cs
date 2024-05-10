using System.Threading.Tasks;
using System.Web.Mvc;
using Facilit.Models;
using Facilit.Servicos;

namespace Facilit.Controllers
{
    public class EtiquetasController : Controller
    {
        public ActionResult Etiquetas()
        {
            return View();
        }

        public ActionResult ObterEtiquitas()
        {
            return View();
        }
        public async Task<ActionResult> ObterLinkEtiquita(EtiquetasTiny tiny)
        {
            var TinyApi = new RetornoTinyApi();
      
            string link = await TinyApi.ObterEtiqueta(tiny);

            if (!string.IsNullOrEmpty(link))
            {

                return Redirect(link);
            }
            else
            {

                return View("Error");
            }
        }

        //public async Task<ActionResult> ObterExpedicao(EtiquetasTiny etiquetas)
        //{
        //    RetornoTinyApi retornoTiny = new RetornoTinyApi();
        //    var expedicoes = await retornoTiny.PesquisaExpedicao(etiquetas);

        //    ViewBag.Expedicoes = expedicoes; 

        //    return RedirectToAction("Escolha"); 
        //}

        //public ActionResult Escolha()
        //{
        //    return View();
        //}
    }
}
