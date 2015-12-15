using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DotNet.Framework.Common.Helpers
{
    public static class SerializationHelper
    {
        public static T XmlSerializerXmlToObject<T>(string Xml)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T Object = XmlSerializerXmlToObject<T>(Xml, serializer);
            return Object;
        }
        public static T XmlSerializerXmlToObject<T>(string Xml, XmlSerializer serializer)
        {
            StringReader stringReader = new StringReader(Xml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (T)serializer.Deserialize(xmlReader);
        }
        public static string XmlSerializerObjectToXml<T>
                                    (
                                        T Object
                                        , XmlTextWriter writer
                                        , XmlSerializer serializer
                                    )
        {
            serializer.Serialize(writer, Object);
            MemoryStream stream = writer.BaseStream as MemoryStream;
            byte[] bytes = stream.ToArray();
            Encoding e = EncodingHelper.IdentifyEncoding
                                            (
                                                bytes
                                                , Encoding.GetEncoding("gb2312")
                                            ///                                                , new Encoding[]
                                            ///                                                        {
                                            ///                                                            Encoding.UTF8
                                            ///                                                            , Encoding.Unicode
                                            ///                                                        }
                                            );
            byte[] buffer = e.GetPreamble();
            int offset = buffer.Length;
            buffer = new byte[bytes.Length - offset];
            Buffer.BlockCopy(bytes, offset, buffer, 0, buffer.Length);
            string s = e.GetString(buffer);
            return s;
        }
        public static string XmlSerializerObjectToXml<T>(T Object, XmlSerializer serializer)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Encoding e = Encoding.UTF8;
                XmlTextWriter writer = new XmlTextWriter(stream, e);
                string s = XmlSerializerObjectToXml<T>
                                    (
                                        Object
                                        , writer
                                        , serializer
                                    );
                writer.Close();
                writer = null;
                return s;
            }
        }
        public static string XmlSerializerObjectToXml<T>(T Object, Encoding e, XmlSerializer serializer)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(stream, e);
                string s = XmlSerializerObjectToXml<T>
                                    (
                                        Object
                                        , writer
                                        , serializer
                                    );
                writer.Close();
                writer = null;
                return s;
            }
        }
        public static string XmlSerializerObjectToXml<T>(T Object, Encoding e)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlTextWriter writer = new XmlTextWriter(stream, e);
                string s = XmlSerializerObjectToXml<T>
                                    (
                                        Object
                                        , writer
                                        , serializer
                                    );
                writer.Close();
                writer = null;
                return s;
            }
        }
        public static byte[] FormatterObjectToBinary<T>(T Object)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formater = new BinaryFormatter();
                formater.Serialize(stream, Object);
                byte[] buffer = stream.ToArray();
                return buffer;
            }
        }
        public static T FormatterBinaryToObject<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formater = new BinaryFormatter();
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                T Object = (T)formater.Deserialize(stream);
                return Object;
            }
        }

        public static string ObjectToJSon<T>(T obj)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, obj);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static T JSonToObject<T>(this string self)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(self));
            T obj = (T)serializer.ReadObject(ms);
            return obj;
        }

        #region Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            try
            {
                char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
                                            'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
                                            'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
                                            '8','9','+','/','='};
                byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                StringBuilder outmessage;
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use = 0;
                if ((use = messageLen % 3) > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                outmessage = new System.Text.StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            //将空格替换为加号
            text = text.Replace(" ", "+");

            try
            {
                if ((text.Length % 4) != 0)
                {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }
                string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.Default.GetString(outbyte);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region 使用指定字符集将string转换成byte[]
        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }
        #endregion

        #region 使用指定字符集将byte[]转换成string
        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }
        #endregion
    }
}
