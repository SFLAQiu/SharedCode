using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;

namespace LG.Utility {
    public static class PublicExtend {
        /// <summary>
        /// 尽最大努力取得用户IP
        /// </summary>
        /// <returns>用户IP</returns>
        public static string GetClientRealIp() {
            var Request = HttpContext.Current.Request;
            if (Request == null) return "";
            try {
                string returnValue = "";
                if (Request.ServerVariables["HTTP_VIA"] != null) {
                    returnValue = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }

                //因为我们的调度服务器没有HTTP_VIA值，所以加入这段代码进行修复
                if (string.IsNullOrEmpty(returnValue)) {
                    if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                        returnValue = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }

                if (string.IsNullOrEmpty(returnValue)) {
                    returnValue = HttpContext.Current.Request.UserHostAddress;
                }

                if (returnValue == "::1")
                    returnValue = "127.0.0.1";

                return returnValue;
            } catch {
                return "0.0.0.0";
            }

        }
        /// <summary>
        /// 集合转为符号隔开的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas">集合</param>
        /// <param name="separator">符号</param>
        /// <returns></returns>
        public static string ToJoinStr<T>(this List<T> datas, string separator) where T : struct {
            if (datas == null || datas.Count <= 0) return string.Empty;
            return string.Join(separator, datas);
        }
        /// <summary>
        /// 获取随机种子码
        /// </summary>
        /// <returns></returns>
        public static int GetRandSeed() {
            byte[] bytes = new byte[8];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

    }
}
