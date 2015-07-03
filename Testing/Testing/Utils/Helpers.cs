using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Testing.Utils
{
    public static class Helpers
    {
        public static bool IsVietnamese()
        {
           // string ip = HttpContext.Current.Request.UserHostAddress;
            string ip = "42.117.67.100";
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