using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facilit.Controllers
{
    public class HomeController : Controller
    {
       public ActionResult Index()
        {
            ViewBag.MostrarBotoes = true;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.MostrarBotoes = true;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.MostrarBotoes = true;
            return View();
        }
    }
}