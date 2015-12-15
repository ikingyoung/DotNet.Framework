using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Extensions.StringValidation
{
    public static class StringValidate
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        //等价于^[+-]?\d+[.]?\d+$   
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
        //w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样    
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegIP = new Regex(@"(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])");

        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }
        public static bool IsNullOrWhiteSpace(this string self)
        {
            return string.IsNullOrWhiteSpace(self);
        }
        /// <summary>   
        /// 是否数字字符串   
        /// </summary>   
        /// <param name="self">输入字符串</param>   
        /// <returns></returns>   
        public static bool IsNumber(this string self)
        {
            var m = RegNumber.Match(self);
            return m.Success;
        }
        /// <summary>   
        /// 是否数字字符串 可带正负号   
        /// </summary>   
        /// <param name="self">输入字符串</param>   
        /// <returns></returns>   
        public static bool IsNumberSign(this string self)
        {
            var m = RegNumberSign.Match(self);
            return m.Success;
        }
        /// <summary>   
        /// 是否是浮点数   
        /// </summary>   
        /// <param name="self">输入字符串</param>   
        /// <returns></returns>   
        public static bool IsDecimal(this string self)
        {
            var m = RegDecimal.Match(self);
            return m.Success;
        }
        /// <summary>   
        /// 是否是浮点数 可带正负号   
        /// </summary>   
        /// <param name="self">输入字符串</param>   
        /// <returns></returns>   
        public static bool IsDecimalSign(this string self)
        {
            var m = RegDecimalSign.Match(self);
            return m.Success;
        }
        /// <summary>   
        /// 检测是否有中文字符   
        /// </summary>   
        /// <param name="self"></param>   
        /// <returns></returns>   
        public static bool IsHasChineseString(this string self)
        {
            var m = RegCHZN.Match(self);
            return m.Success;
        }
        /// <summary>   
        /// 检测是否为邮件地址   
        /// </summary>   
        /// <param name="self"></param>   
        /// <returns></returns>  
        public static bool IsEmail(this string self)
        {
            var m = RegEmail.Match(self);
            return m.Success;
        }
        /// <summary>
        /// check whether the ip is valid
        /// </summary>
        /// <param name="p_IP">IP string</param>
        /// <returns>True/False</returns>
        public static bool IsIP(this string self)
        {

            if (RegIP.IsMatch(self))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否为日期
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string self)
        {
            DateTime d;
            return DateTime.TryParse(self, out d);
        }
        /// <summary>
        /// 是否匹配正则表达式
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }
        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }
    }
}
