using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Extensions.Object
{
    public static class ObjectConvertable
    {
        public static Int32 ToInt32(object obj, int defaultValue = 0)
        {
            int result;
            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = obj.ToString();
                if (!Int32.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        public static Int64 ToInt64(object obj, Int64 defaultValue = 0)
        {
            long result;
            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = obj.ToString();
                if (!Int64.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        public static Decimal ToDecimal(object obj, Decimal defaultValue = 0)
        {
            Decimal result;
            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = obj.ToString();
                if (!Decimal.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        public static Decimal ToDecimal(object obj, int decimals, Decimal defaultValue)
        {
            Decimal result;

            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                var tmpValue = ToDecimal(obj).ToString();
                if (tmpValue.IndexOf('.') > -1)
                {
                    var totalLen = tmpValue.Length;
                    var subLen = totalLen - (tmpValue.IndexOf('.') + 1);

                    if (decimals >= subLen)
                    {

                    }
                    else
                    {
                        tmpValue = tmpValue.Substring(0, totalLen - subLen + decimals);
                    }
                }

                if (!Decimal.TryParse(tmpValue, out result))
                {
                    result = defaultValue;
                }
            }

            return result;
        }
        public static Double ToDouble(object obj, Double defaultValue = 0)
        {
            Double result;
            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = obj.ToString();
                if (!Double.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }

            return result;
        }
        public static Double ToDouble(object obj, int decimals, Double defaultValue)
        {
            Double result;
            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = obj.ToString();
                var tmpValue = ToDouble(value).ToString();
                if (tmpValue.IndexOf('.') > -1)
                {
                    var totalLen = tmpValue.Length;
                    var subLen = totalLen - (tmpValue.IndexOf('.') + 1);
                    if (decimals >= subLen)
                    {
                    }
                    else
                    {
                        tmpValue = tmpValue.Substring(0, totalLen - subLen + decimals);
                    }
                }

                if (!Double.TryParse(tmpValue, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        public static string ToString(object obj)
        {
            if (obj == null)
                return string.Empty;
            return obj.ToString();
        }
    }
}
