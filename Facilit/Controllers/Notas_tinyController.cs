using Facilit.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facilit.Models;
namespace Facilit.Controllers
{
    public class Notas_tinyController : Controller
    {
        // GET: Notas_tiny
        public ActionResult Notas()
        {
            if (Session["logado"] == null)
            {
                
                return RedirectToAction("Index", "usuario");
            }

           
            return View();
        }

        RetornoTinyApi TinyApi = new RetornoTinyApi();
        public async Task<ActionResult> ObterNotas(NF_Tiny tiny)
        {
            NF_Tiny nf = new NF_Tiny { numero = tiny.numero };

            string link = await TinyApi.ConsultaNota(nf);

          
            if (!string.IsNullOrEmpty(link))
            {
               
                return Redirect(link);
            }
            else
            {
              
                return View("Error");
            }
        }

    }

}