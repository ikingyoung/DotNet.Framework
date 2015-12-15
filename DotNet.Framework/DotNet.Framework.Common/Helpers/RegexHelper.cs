using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Helpers
{
    public static class RegexHelper
    {
        /// <summary>
        /// 提取中括号的内容（包括中括号）
        /// </summary>
        public static readonly string MatchMidBracket = @"\[(.*?)\]";
        /// <summary>
        /// 提取大括号的内容（包括大括号）
        /// </summary>
        public static readonly string MatchBigBracket = @"\{(.*?)\}";
        /// <summary>
        /// 提取小括号的内容（包括小括号）
        /// </summary>
        public static readonly string MatchSmallBracket = @"\((.*?)\)";
        /// <summary>
        /// 提取尖括号的内容（包括尖括号）
        /// </summary>
        public static readonly string MatchAngleBracket = @"\<(.*?)\>";
        /// <summary>
        /// 是否为数字
        /// </summary>
        public static readonly string IsDigit = @"^[+-]?[0-9]+[.]?[0-9]+$";
        /// <summary>
        /// 非负整数（正整数 + 0）
        /// </summary>
        public static readonly string IsIntNosign = @"^[0-9]+$";
        /// <summary>
        /// 是否包含后缀(.com|.net|.org|.edu|.mil|.tv|.biz|.info)
        /// </summary>
        public static readonly string HasDomainNameSuffix = @"^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$";
        /// <summary>
        /// 是否为中文字串
        /// </summary>
        public static readonly string IsChineseCharacter = @"^[\u4e00-\u9fa5],{0,}$";//@"[\u4e00-\u9fa5]";
        /// <summary>
        /// 是否为IP
        /// </summary>
        public static readonly string IsIP = @"(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])";
        /// <summary>
        /// 是否为Email
        /// </summary>
        public static readonly string IsEmail = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
        /// <summary>
        /// 是否为Url
        /// </summary>
        public static readonly string IsUrl = @"^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$";
        /// <summary>
        /// 只能输入由26个英文字母组成的字符串
        /// </summary>
        public static readonly string IsEnglistCharacter = @"^[A-Za-z]+$";
        /// <summary>
        /// 只能输入由26个大写英文字母组成的字符串
        /// </summary>
        public static readonly string IsEnglishUpcaseCharacter = @"^[A-Z]+$";
        /// <summary>
        /// 只能输入由26个小写英文字母组成的字符串
        /// </summary>
        public static readonly string IsEnglishLowcaseCharacter = @"^[a-z]+$";
        /// <summary>
        /// 只能输入由数字和26个英文字母组成的字符串
        /// </summary>
        public static readonly string IsNumberAndEnglistCharacter = @"^[A-Za-z0-9]+$";
        /// <summary>
        /// 只能输入由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        public static readonly string IsNumberAndEnglistCharacterAndUnderline = @"^\w+$";
        /// <summary>
        /// 验证用户密码
        /// <remarks>
        /// 正确格式为：以字母开头，长度在6-18之间,只能包含字符、数字和下划线。
        /// </remarks>
        /// </summary>
        public static readonly string MatchPasswordRule = @"^[a-zA-Z]\w{5,17}$";
        /// <summary>
        /// 验证是否含有^%&',;=?$\"等字符
        /// </summary>
        public static readonly string ExsitSpecailSymbol = @"[^%&',;=?$\x22]+";
        /// <summary>
        /// 验证电话号码
        /// <remarks>
        /// 正确格式为："XXXX-XXXXXXX"，“XXXX-XXXXXXXX"，“XXX-XXXXXXX"，"XXX-XXXXXXXX"，“XXXXXXX"，“XXXXXXXX"
        /// </remarks>
        /// </summary>
        public static readonly string IsPhoneNumber = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$";


    }
}
