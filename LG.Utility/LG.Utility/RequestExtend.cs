using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LG.Utility {
    public static class RequestExtend {
        /// <summary>
        /// 获取Request.QueryString的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQ(this HttpRequest request,string key){
            string str = request.QueryString[key];
            if (string.IsNullOrEmpty(str)) return "";
            return str;
        }
        /// <summary>
        /// 获取Request.Form的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetF(this HttpRequest request, string key) {
            string str = request.Form[key];
            if (string.IsNullOrEmpty(str)) return "";
            return str;
        }
        /// <summary>
        /// 根据Get请求是否有传callBack参数，返回跨域访问的callBack(..)格式字符串
        /// </summary>
       public static Func<string, string>  GetCallBackStr = new Func<string, string>(delegate(string rtnStr) {
            string sellerInfoJsonCallBack = "{0}({1})";
            string callBack = HttpContext.Current.Request.QueryString["callback"];
            if (!string.IsNullOrWhiteSpace(callBack)) return string.Format(sellerInfoJsonCallBack, callBack, rtnStr);
            return rtnStr;
        });
    }
}
