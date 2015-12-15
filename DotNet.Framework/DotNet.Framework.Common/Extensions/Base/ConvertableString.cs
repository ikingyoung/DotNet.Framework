using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Framework.Common.Extensions
{
    //public interface IConvertableString : IExtension<string> { }

    //public static class ConvertableString
    //{
    //    public static Int32 ToInt32(this IConvertableString s, Int32 defaultValue = 0)
    //    {
    //        int result;
    //        if (!Int32.TryParse(s.Self, out result))
    //        {
    //            result = defaultValue;
    //        }

    //        return result;
    //    }
    //    public static Int64 ToInt64(this IConvertableString s, Int64 defaultValue = 0)
    //    {
    //        long result;
    //        if (!Int64.TryParse(s.Self, out result))
    //        {
    //            result = defaultValue;
    //        }

    //        return result;
    //    }
    //    public static Decimal ToDecimal(this IConvertableString s, Decimal defaultValue = 0)
    //    {
    //        Decimal result;
    //        if (string.IsNullOrEmpty(s.Self))
    //        {
    //            result = defaultValue;
    //        }
    //        else
    //        {
    //            var value = s.Self;
    //            if (!Decimal.TryParse(value, out result))
    //            {
    //                result = defaultValue;
    //            }
    //        }
    //        return result;
    //    }
    //    public static Decimal ToDecimal(this IConvertableString s,int decimals, Decimal defaultValue)
    //    {
    //        Decimal result;

    //        if (string.IsNullOrEmpty(s.Self))
    //        {
    //            result = defaultValue;
    //        }
    //        else
    //        {
    //            var tmpValue = s.Self;
    //            if (tmpValue.IndexOf('.') > -1)
    //            {
    //                var totalLen = tmpValue.Length;
    //                var subLen = totalLen - (tmpValue.IndexOf('.') + 1);

    //                if (decimals >= subLen)
    //                {

    //                }
    //                else
    //                {
    //                    tmpValue = tmpValue.Substring(0, totalLen - subLen + decimals);
    //                }
    //            }

    //            if (!Decimal.TryParse(tmpValue, out result))
    //            {
    //                result = defaultValue;
    //            }
    //        }

    //        return result;
    //    }        
    //    public static Double ToDouble(this IConvertableString s, Double defaultValue = 0)
    //    {
    //        Double result;
    //        if (string.IsNullOrEmpty(s.Self))
    //        {
    //            result = defaultValue;
    //        }
    //        else
    //        {
    //            var value = s.Self;
    //            if (!Double.TryParse(value, out result))
    //            {
    //                result = defaultValue;
    //            }
    //        }

    //        return result;
    //    }
    //    public static Double ToDouble(this IConvertableString s, int decimals, Double defaultValue)
    //    {
    //        Double result;
    //        if (string.IsNullOrEmpty(s.Self))
    //        {
    //            result = defaultValue;
    //        }
    //        else
    //        {
    //            var value = s.Self;
    //            var tmpValue = value.As<IConvertableString>().ToDouble(defaultValue).ToString();
    //            if (tmpValue.IndexOf('.') > -1)
    //            {
    //                var totalLen = tmpValue.Length;
    //                var subLen = totalLen - (tmpValue.IndexOf('.') + 1);
    //                if (decimals >= subLen)
    //                {
    //                }
    //                else
    //                {
    //                    tmpValue = tmpValue.Substring(0, totalLen - subLen + decimals);
    //                }
    //            }

    //            if (!Double.TryParse(tmpValue, out result))
    //            {
    //                result = defaultValue;
    //            }
    //        }
    //        return result;
    //    }
    //    public static DateTime? ToDateTime(this IConvertableString s,DateTime? defaultValue =null)
    //    {
    //        DateTime result;
    //        if (!DateTime.TryParse(s.Self, out result))
    //        {
    //            return defaultValue;
    //        }
    //        return result;
    //    }
    //}
}
