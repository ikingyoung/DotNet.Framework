using System.IO;
using System.Text;
using System.Xml;

namespace DotNet.Framework.Common.Helpers
{
    public static class XMLHelper
    {
        /// <summary>
        /// 格式化XML
        /// </summary>
        /// <param name="sUnformattedXml"></param>
        /// <returns></returns>
        public static string FormatXml(string sUnformattedXml)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXml);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);

                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '\t';

                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }
    }
}
