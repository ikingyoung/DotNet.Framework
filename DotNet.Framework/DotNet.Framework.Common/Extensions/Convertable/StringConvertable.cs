using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Extensions.Convertable.String
{
    public enum AryType
    {
        TwoAry=2,
        EightAry=8,
        TenAry=10,
        Sixteen=16
    }

    public static class StringConvertable
    {
        #region 各进制数间转换
        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="self">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ToXAry(this string self, AryType from, AryType to)
        {
            try
            {
                int intValue = Convert.ToInt32(self, (int)from);  //先转成10进制
                string result = Convert.ToString(intValue, (int)to);  //再转成目标进制
                if ((int)to == 2)
                {
                    int resultLength = result.Length;  //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {
                return "0";
            }
        }
        #endregion

        #region 日期处理
        

        #endregion 
    }
}
