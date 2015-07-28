using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testing.Controllers
{
    public class AppController : Controller
    {
        //hashKey
        private const string KEY = "dfamd175azxhm59a0075ips1a82";
        private static Object _lock = new object();
        // GET: Api
        public string GetSnakeHighScore()
        {
            string key = Request.QueryString["key"];
            if (string.IsNullOrEmpty(key)||key!="dfamd175azxhm5900075ips1a82")
            {
                return "0";
            }
            string highscore="0";
            string filePath = Server.MapPath("~/Games/Snake/highscore.data");
            if (!System.IO.File.Exists(filePath))
            {
                FileStream filestream = null;
                filestream=System.IO.File.Create(filePath);
                filestream.Close();
                highscore = "0";
                System.IO.File.WriteAllText(filePath, highscore);
                
            }
            else
            {
                lock (_lock)
                {
                    highscore = System.IO.File.ReadAllText(filePath);
                }
            }
            return highscore;
        }
        //Post
        public void PostSnakeHighScore()
        {

        }

        private string GenerateKey(string score)
        {
            string hkey = "";
            string[] a = KEY.Split('a');
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i]=="2")
                {
                    a[i] = a[i] + score;
                }
                hkey += a[i];
            }
            return hkey;
        }
    }
}