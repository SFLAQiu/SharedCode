using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace LG.Utility {
    public static class XMLExtend {
        /// <summary>
        /// 更具xml节点获取节点中的值，如果为空时返回默认值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValue(this XElement ele, string defaultValue = "") {
            if (ele == null) return defaultValue;
            return ele.Value.GetString(defaultValue);
        }
        /// <summary>
        /// 更具xml节点属性获取属性中的值，如果为空时返回默认值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValue(this XAttribute at, string defaultValue = "") {
            if (at == null) return defaultValue;
            return at.Value.GetString(defaultValue);
        }
        #region  "将XML字符串反序列化为对象，将对象序列化为XML字符串"

        private static void XmlSerializeInternal(Stream stream, object obj, Encoding encoding) {
            if (obj == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
                serializer.Serialize(writer, obj);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串（和XmlDeserialize对应）
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(this object obj, Encoding encoding) {
            using (MemoryStream stream = new MemoryStream()) {
                XmlSerializeInternal(stream, obj, encoding);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding)) {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件(和XmlDeserializeFromFile对应)
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="path">保存文件路径（包含文件名）</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(this object obj, string path, Encoding encoding) {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                XmlSerializeInternal(file, obj, encoding);
            }
        }


        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="xmlString">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(this string xmlString, Encoding encoding) {
            if (string.IsNullOrEmpty(xmlString))
                throw new ArgumentNullException("xmlString");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xmlString))) {
                using (StreamReader sr = new StreamReader(ms, encoding)) {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(this string filePath, Encoding encoding) {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(filePath, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }
        #endregion
    }
}
