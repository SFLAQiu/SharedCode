using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SharedHelp.Net40 {
    public static class StringExtend {
        #region "各种"
        /// <summary>
        /// string 类型转为其他值类型
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="str">字符串</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static T To<T>(this string str, T defaultVal = default(T)) where T : struct {
            try {
                if (string.IsNullOrEmpty(str)) return defaultVal;
                return (T)Convert.ChangeType(str, typeof(T));
            } catch {
                return defaultVal;
            }

        }
        /// <summary>
        /// System.String 对象是 null 还是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 复合格式字符串,{0},{2}...
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatStr(this string str, params object[] args) {
            if(string.IsNullOrEmpty(str))return "";
            return string.Format(str, args);
        }
        #endregion
        #region "Serialize"
        /// <summary>
        /// 从字符串中序列化对象(和SerializeToString对应),出错会抛出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object DeserializeObject(this string input) {
            IFormatter formatter = new BinaryFormatter();
            object obj = null;
            byte[] byt = Convert.FromBase64String(input);
            using (Stream stream = new MemoryStream(byt, 0, byt.Length)) {
                obj = formatter.Deserialize(stream);
            }
            return obj;
        }
        #endregion
        #region "JSON"
        /// <summary>
        /// 根据JSON字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JSONDeserialize<T>(this string jsonStr) {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Deserialize<T>(jsonStr);
        }
        #endregion
    }
}
