using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facilit.Models;
using Facilit.Servicos;

namespace Facilit.Controllers
{
    public class EtiquetasController : Controller
    {
        // GET: Etiquetas
        public ActionResult Etiquetas()
        {
            return View();
        }
        public async Task ObterExpedicao(EtiquetasTiny etiquetas)
        {
            EtiquetasTiny etiquetasTiny = new EtiquetasTiny();
            RetornoTinyApi retornoTiny =new RetornoTinyApi();
           await retornoTiny.PesquisaExpedicao(etiquetas);
        }
    }
}