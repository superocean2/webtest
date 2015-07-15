using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testing.Models;
using System.Drawing.Text;
using System.Drawing.Imaging;

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
            var urlpicture = Server.MapPath("~/content/userfiles/images/test.png");
            var urlText = Server.MapPath("~/content/texts/test1.txt");
            string text = System.IO.File.ReadAllText(urlText);
            string[] textLines = text.Split('\n');
            int textLine = Testing.Utils.Helpers.GetRandomInteger(0, textLines.Length);
            var fonts = new PrivateFontCollection();
            fonts.AddFontFile(Server.MapPath("~/content/fonts/chuviet1.ttf"));
            Font myfont = new Font((FontFamily)fonts.Families[0], 20f);
            Image image = Testing.Utils.Helpers.DrawText(urlpicture, textLines[textLine], myfont, Color.Black,220,130,30);
            string filename = Testing.Utils.Helpers.RandomString() + ".jpg";
            string browsePath = "~/content/images/users/" + filename;
            string savePath = Server.MapPath(browsePath);
            image.Save(savePath, ImageFormat.Jpeg);
            image.Dispose();
            ViewBag.url = browsePath;
            return View();
        }
    }
}