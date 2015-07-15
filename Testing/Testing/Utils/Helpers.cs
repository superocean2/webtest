using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
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


        public static string Serialize(object oObject, bool Indent = false)
        {

            System.Xml.Serialization.XmlSerializer oXmlSerializer = null;
            System.Text.StringBuilder oStringBuilder = null;
            System.Xml.XmlWriter oXmlWriter = null;
            string sXML = null;
            System.Xml.XmlWriterSettings oXmlWriterSettings = null;
            System.Xml.Serialization.XmlSerializerNamespaces oXmlSerializerNamespaces = null;

            // -----------------------------------------------------------------------------------------------------------------------
            // Lage XML
            // -----------------------------------------------------------------------------------------------------------------------
            oStringBuilder = new System.Text.StringBuilder();
            oXmlSerializer = new System.Xml.Serialization.XmlSerializer(oObject.GetType());
            oXmlWriterSettings = new System.Xml.XmlWriterSettings();
            oXmlWriterSettings.OmitXmlDeclaration = true;
            oXmlWriterSettings.Indent = Indent;
            oXmlWriter = System.Xml.XmlWriter.Create(new System.IO.StringWriter(oStringBuilder), oXmlWriterSettings);
            oXmlSerializerNamespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
            oXmlSerializerNamespaces.Add(string.Empty, string.Empty);
            oXmlSerializer.Serialize(oXmlWriter, oObject, oXmlSerializerNamespaces);
            oXmlWriter.Close();
            sXML = oStringBuilder.ToString();

            return sXML;
        }

        public static object DeSerialize(string sXML, Type ObjectType)
        {
            System.IO.StringReader oStringReader = null;
            System.Xml.Serialization.XmlSerializer oXmlSerializer = null;
            object oObject = null;

            // -----------------------------------------------------------------------------------------------------------------------
            // Hvis mangler info, lage tom
            // -----------------------------------------------------------------------------------------------------------------------
            if (sXML == string.Empty)
            {
                Type[] types = new Type[-1 + 1];
                ConstructorInfo info = ObjectType.GetConstructor(types);
                object targetObject = info.Invoke(null);
                if (targetObject == null)
                    return null;
                return targetObject;
            }

            // -----------------------------------------------------------------------------------------------------------------------
            // Gjøre om fra XML til objekt
            // -----------------------------------------------------------------------------------------------------------------------
            oStringReader = new System.IO.StringReader(sXML);
            oXmlSerializer = new System.Xml.Serialization.XmlSerializer(ObjectType);

            try
            {
                oObject = oXmlSerializer.Deserialize(oStringReader);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + "\nStacktrace:\n" + ex.InnerException.StackTrace + "\nXML:\n" + sXML));
            }

            return oObject;
        }


        public static ulong GetRandomLong()
        {
            Random rnd = null;
            if (rnd == null) rnd = new Random((int)DateTime.Now.Ticks);
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        public static int GetRandomInteger()
        {
            Random rnd = null;
            if (rnd == null) rnd = new Random((int)DateTime.Now.Ticks);
            return Math.Abs(Convert.ToInt32(rnd.Next(int.MaxValue - 10000) + 10000));

            //return Convert.ToInt32(Math.Abs(10000 + (VBMath.Rnd() * (int.MaxValue - 10000))));
        }

        public static int GetRandomInteger(int minValue, int maxValue)
        {
            Random rnd = null;
            if (rnd == null) rnd = new Random((int)DateTime.Now.Ticks);
            return Convert.ToInt32(rnd.Next(minValue, maxValue));
        }


        public static string RandomString()
        {
            Random rnd = new Random();
            int seed = rnd.Next(1, int.MaxValue);
            const string allowedChars = "abcdefghijkmnopqrstuvwxyz0123456789";

            var chars = new char[11];
            var rd = new Random(seed);

            for (var i = 0; i < 11; i++)
            {

                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];

            }

            return new string(chars);
        }



        public static string PasswordGenerator(int passwordLength, bool strongPassword)
        {
            Random rnd = new Random();
            int seed = rnd.Next(1, int.MaxValue);
            //const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string allowedChars = "abcdefghijkmnopqrstuvwxyz0123456789";
            const string specialCharacters = @"!#$%&'()*+,-./:;<=>?@[\]_";

            var chars = new char[passwordLength];
            var rd = new Random(seed);

            for (var i = 0; i < passwordLength; i++)
            {
                // If we are to use special characters
                if (strongPassword && i % rnd.Next(3, passwordLength) == 0)
                {
                    chars[i] = specialCharacters[rd.Next(0, specialCharacters.Length)];
                }
                else
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
            }

            return new string(chars);
        }

        public static Image DrawText(string pictureUrl,String text, Font font, Color textColor,int x,int y,int spacing)
        {
            Image img = new Bitmap(pictureUrl);
            Graphics drawing = Graphics.FromImage(img);
            drawing.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);
           
            string[] arr = text.Split(';');
            for (int i = 0; i < arr.Length;i++ )
            {
                drawing.DrawString(arr[i], font, textBrush, x, y+i*spacing);
            }
            

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            return img;
        }
    }
}