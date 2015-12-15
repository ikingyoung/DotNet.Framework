using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Convertable.Object
{
    public static class ObjectConvertable
    {
        public static Int32 ToInt32(this object self, int defaultValue = 0)
        {
            int result;
            if (self == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = self.ToString();
                if (!Int32.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        public static Int64 ToInt64(this object self, Int64 defaultValue = 0)
        {
            long result;
            if (self == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = self.ToString();
                if (!Int64.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        public static Decimal ToDecimal(this object self, Decimal defaultValue = 0)
        {
            Decimal result;
            if (self == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = self.ToString();
                if (!Decimal.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        public static Decimal ToDecimal(this object self, int decimals, Decimal defaultValue)
        {
            Decimal result;

            if (self == null)
            {
                result = defaultValue;
            }
            else
            {
                var tmpValue = ToDecimal(self).ToString();
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
        public static Double ToDouble(this object self, Double defaultValue = 0)
        {
            Double result;
            if (self == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = self.ToString();
                if (!Double.TryParse(value, out result))
                {
                    result = defaultValue;
                }
            }

            return result;
        }
        public static Double ToDouble(this object self, int decimals, Double defaultValue)
        {
            Double result;
            if (self == null)
            {
                result = defaultValue;
            }
            else
            {
                var value = self.ToString();
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
        public static string ToString(this object self)
        {
            if (self == null)
                return string.Empty;
            return self.ToString();
        }
    }

}
