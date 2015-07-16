using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testing.Models;

namespace Testing.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EyeTest()
        {
            return View();
        }

        public ActionResult AlienSaid()
        {
            ViewBag.pageUrl = "aliensaid";
            return View();

        }

        [HttpPost]
        public string AlienSaidPost(NameSex name)
        {
            return name.Name;
        }
    }
}
