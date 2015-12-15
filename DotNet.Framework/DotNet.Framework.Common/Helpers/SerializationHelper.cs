using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
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
    }
}
