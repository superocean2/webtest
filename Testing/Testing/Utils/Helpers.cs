using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Testing.Utils
{
    public class FaceBookAPI
    {
        public string AppID { get; set; }
        public string AppSecret { get; set; }
    }
    public static class Helpers
    {
        public static FaceBookAPI GetFacebookAPI()
        {
            return new FaceBookAPI() { AppID = "410938229108210", AppSecret = "1e6856af57bde3004223b6aba5fbc292" };
        }
        public static string GetIP()
        {
            string ip = GetIPAddress();
            string country = CountryFromIp(ip).ToLower();
            return ip + " .Country: " + country;
        }
        private static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static bool IsVietnamese()
        {
           string ip = HttpContext.Current.Request.UserHostAddress;
            //string ip = "42.117.67.100";
            if (CountryFromIp(ip).ToLower().Equals("vn"))
            {
                return true;
            }

            return false;
        }

        private static string CountryFromIp(string ip)
        {
            try
            {
                string csvfile = HttpContext.Current.Server.MapPath("~/content/Ip2Country.csv");
                string content = System.IO.File.ReadAllText(csvfile);
                string[] ips = ip.Split('.');
                if (ips.Length != 4)
                {
                    return "VN";
                }
                double number = 16777216 * int.Parse(ip.Split('.')[0]) + 65536 * int.Parse(ip.Split('.')[1]) + 256 * int.Parse(ip.Split('.')[2]) + int.Parse(ip.Split('.')[3]);
                string[] ranges = content.Split('\n');

                foreach (var range in ranges)
                {
                    string[] aranges = range.Split(',');
                    double start = double.Parse(aranges[0].Trim('"'));
                    double end = double.Parse(aranges[1].Trim('"'));
                    if (start<=number&& number<=end)
                    {
                        return aranges[2].Trim('"');
                    }
                }
            }
            catch (Exception)
            {

                return "VN";
            }
            

            return "VN";
        }
    }
}