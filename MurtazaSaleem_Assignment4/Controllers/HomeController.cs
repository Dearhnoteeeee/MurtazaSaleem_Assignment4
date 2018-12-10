using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurtazaSaleem_Assignment4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Recievables for Invoice app
        /// </summary>
        /// <returns></returns>
        public ActionResult Recievables()
        {
            ViewBag.Message = "Your recievable page.";

            return View();
        }

        public ActionResult InvoiceForm()
        {
            ViewBag.Message = "Your Invoice Form page.";

            return View();
        }
    }
}