using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankOfBIT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //change the Message after "Home Page." in the home page
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ViewBag.Message = "Online Banking for the Bank of BIT clients";

            return View();
        }

        public ActionResult About()
        {
            //change the Message after "About." in the home page
            //ViewBag.Message = "Your app description page.";
            ViewBag.Message = "Bank of BIT Application.";

            return View();
        }

        public ActionResult Contact()
        {
            //change the Message after "Contact." in the home page
            //ViewBag.Message = "Your contact page.";
            ViewBag.Message = "Find our Contact Information here.";

            return View();
        }
    }
}
