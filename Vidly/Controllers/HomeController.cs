using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Vidly.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            logger.Info("HomeController->Index: Called" + Environment.NewLine + DateTime.Now);
            return RedirectToAction("Index", "AdminPanel");
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return RedirectToAction("Index", "AdminPanel");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return RedirectToAction("Index", "AdminPanel");
        }
    }
}