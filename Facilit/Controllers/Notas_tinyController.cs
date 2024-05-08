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

        public async Task<ActionResult> ObterNotas(NF_Tiny tiny)
        {
            var TinyApi = new RetornoTinyApi();
            string link = await TinyApi.ConsultaNota(tiny);

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