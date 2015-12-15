using System;
using System.Text.RegularExpressions;

namespace DotNet.Framework.Common.Extensions.StringFormat
{
    public static class StringFormat
    {
        public static string FormatWith(this string self, params object[] args)
        {
            return string.Format(null,self, args);
        }
        public static string FormatWith(this string self, object arg0)
        {
            return string.Format(null, self, new object[] { arg0 });
        }
        public static string FormatWith(this string self, object arg0, object arg1)
        {
            return string.Format(null, self, new object[] { arg0, arg1 });
        }
        public static string FormatWith(this string self, object arg0, object arg1, object arg2)
        {
            return string.Format(null, self, new object[] { arg0, arg1, arg2 });
        }
        public static string FormatWith(this string self,IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, self, args);
        }        
    }
}
