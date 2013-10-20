using System;
using System.Collections.Generic;
using System.Text;

namespace rptm
{
    static class Extensions
    {
        static public string To64(this string toEncode)
        {
            byte[] tmp = Encoding.UTF8.GetBytes(toEncode);
            string tmp2 = Convert.ToBase64String(tmp);
            return tmp2;
        }

        static public string From64(this string encodedData)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
            }
            catch (Exception e)
            {
                return "<error>";
            }
        }
    }
}
