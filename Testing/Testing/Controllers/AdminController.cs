using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testing.Models;
using System.Drawing.Text;

namespace Testing.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Test test)
        {
            var urlpicture = Server.MapPath("~/content/userfiles/images/images.jpg");
            string text = "this is test for render \\n How can render new test\\n I don't know it";
            Image image = Testing.Utils.Helpers.DrawText(urlpicture, text, new Font(new FontFamily(GenericFontFamilies.SansSerif), 20f), Color.Tomato);
            string filename = Testing.Utils.Helpers.RandomString() + ".jpg";
            string browsePath = "~/content/images/users/" + filename;
            string savePath = Server.MapPath(browsePath);
            image.Save(savePath);
            image.Dispose();
            ViewBag.url = browsePath;
            return View();
        }
    }
}