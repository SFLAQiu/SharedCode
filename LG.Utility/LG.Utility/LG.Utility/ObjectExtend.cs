using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace LG.Utility {
    public static class ObjectExtend {
        #region "类型转换"
        /// <summary>
        /// 从对象中转为int32数据
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是抛出异常(true 抛出异常，false 异常后返回默认值)</param>
        /// <returns></returns>
        public static int GetInt(this object obj, int defaultValue = default(int), bool isThrowException = false) {
            try {
                if(obj == null) return defaultValue;
                return Convert.ToInt32(obj);
            } catch(Exception ex) {
                if(isThrowException) {
                    throw ex;
                } else {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象中转为long(int64)数据
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是抛出异常(true 抛出异常，false 异常后返回默认值)</param>
        /// <returns></returns>
        public static long GetLong(this object obj, long defaultValue = default(long), bool isThrowException = false) {
            try {
                if(obj == null) return defaultValue;
                return Convert.ToInt64(obj);
            } catch(Exception ex) {
                if(isThrowException) {
                    throw ex;
                } else {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象转为string数据
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString(this object obj, string defaultValue = default(string)) {
            if(obj == null) return defaultValue;
            return Convert.ToString(obj);
        }
        /// <summary>
        /// 转为时间类型,并传入默认值,如果为空或者转化失败的话,将返回默认值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this object obj, DateTime defaultValue) {
            var timeStr = obj.GetString("");
            if(timeStr.IsNullOrWhiteSpace()) return defaultValue;
            try {
                return DateTime.Parse(timeStr);
            } catch {
                return defaultValue;
            }
        }
        /// <summary>
        /// 转为时间类型,如果转化失败,将返回null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this object obj) {
            var timeStr = obj.GetString("");
            if(timeStr.IsNullOrWhiteSpace()) return null;
            try {
                return DateTime.Parse(timeStr);
            } catch {
                return null;
            }
        }
        #endregion
        #region "对象格式化"
        /// <summary>
        /// 序列化对象为字符串(和DeserializeObject对应),出错会抛出
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(this object obj) {
            IFormatter formatter = new BinaryFormatter();
            string result = string.Empty;
            using(MemoryStream stream = new MemoryStream()) {
                formatter.Serialize(stream, obj);

                byte[] byt = new byte[stream.Length];
                byt = stream.ToArray();

                result = Convert.ToBase64String(byt);
                stream.Flush();
            }
            return result;
        }
        #endregion
        #region "JSON"
        /// <summary>
        /// 根据对象,获取JSON序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJSON(this Object obj) {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
        #endregion

        #region "JSON 基于 Newtonsoft.Json"
        /// <summary>
        /// 把对象JSON序列化 基于(Newtonsoft.Json)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JSONSerializeV3(this Object obj) {
            string json = JsonConvert.SerializeObject(obj, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return json;
        }
        #endregion
        /// <summary>
        ///串联对象数组的各个元素，其中在每个元素之间使用指定的分隔符。
        /// </summary>
        /// <param name="objs">数组对象</param>
        /// <param name="separator">要用作分隔符的字符串</param>
        /// <returns></returns>
        public static string GetJoinStr(this object[] objs, string separator) {
            if (objs == null) return string.Empty;
            return string.Join(separator, objs);
        }
    }
}
