using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;
using System.IO;
using System.Drawing;
using System.Net;

namespace LG.Utility {
    public static class PublicExtend {
        /// <summary>
        /// 尽最大努力取得用户IP (转移到StaticFunctions，其他地方在调用，这里多写个冗余方法，防止报错)
        /// </summary>
        /// <returns>用户IP</returns>
        public static string GetClientRealIp() {
            return StaticFunctions.GetClientRealIp();
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
      
    }
}
