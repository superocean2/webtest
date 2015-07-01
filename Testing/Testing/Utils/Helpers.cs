using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Testing.Utils
{
    public static class Helpers
    {
        public static string Language(string vi,string en)
        {
            bool isVietnam = true;
            if (isVietnam)
            {
                return vi;
            }
            return en;
        }
    }
}