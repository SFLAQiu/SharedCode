using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace LG.Utility {
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
        /// System.String 对象是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str) {
            return str == string.Empty;
        }
        /// <summary>
        /// System.String 对象不是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string str) {
            return !str.IsEmpty();
        }
        /// <summary>
        /// 前后去空格,如果为null返回string.Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimEx(this string str) {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            else
                return str.Trim();
        }
        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str) {
            return string.IsNullOrWhiteSpace(str);
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
        /// <summary>
        ///  根据Get请求是否有传callBack参数，返回跨域访问的callBack(..)格式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetJsonCallBackStr(this string str) {
            return RequestExtend.GetCallBackStr(str);
        }
        /// <summary>
        /// 获取URL字符串域名(支持获取一级域名或者完整域名)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="needFirstHost"></param>
        /// <returns></returns>
        public static string GetUrlHost(this string url,bool needFirstHost=false) {
            if (url.IsNullOrWhiteSpace()) return string.Empty;
            url= url.ToLower();
            //这里做域名的解析
            var re = new Regex(@"^http:\/\/([^\/]+)\/.*?$");
            if (!re.IsMatch(url)) return string.Empty;
            var host = re.Replace(url, "$1");
            if(!needFirstHost) return host;
            //下面进行以及域名的匹配
            if(host.IsNullOrWhiteSpace()) return host;
            var arrs = host.Split('.');
            if(arrs == null || arrs.Count() <= 0) return string.Empty;
            var allLength=arrs.Length;
            if(allLength < 3) return string.Empty;
            var hostSource = arrs[allLength - 1];
            var hostName = arrs[allLength - 2];
            return hostName + "." + hostSource;
        }
        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="replacement">替换字符串。</param>
        /// <returns></returns>.
        /// 
        public static string RegexReplace(this string str, string pattern, string replacement) {
            if (str.IsNullOrWhiteSpace()) return str;
            return Regex.Replace(str, pattern, replacement);
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
