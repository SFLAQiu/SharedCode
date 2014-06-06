using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LG.Utility {
    public static class RequestExtend {
        #region "Extend Method"
        /// <summary>
        /// 获取Request.QueryString的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQ(this HttpRequest request, string key) {
            string str = request.QueryString[key];
            if(string.IsNullOrEmpty(str)) return "";
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
            if(string.IsNullOrEmpty(str)) return "";
            return str;
        }

        /// <summary>
        /// 获取Request.QueryString的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQ(this HttpRequestBase request, string key) {
            string str = request.QueryString[key];
            if(string.IsNullOrEmpty(str)) return "";
            return str;
        }
        /// <summary>
        /// 获取Request.Form的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetF(this HttpRequestBase request, string key) {
            string str = request.Form[key];
            if(string.IsNullOrEmpty(str)) return "";
            return str;
        }
        /// <summary>
        /// 把form提交的数据集合，绑定到实体T类对应的属性值中
        /// <para>selType=1 从GET请求数据集合中寻找</para>
        /// <para>selType=2 从POST请求数据集合中寻找</para>
        /// <para>selType=3 先从GET，如果没值，则继承从POST</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="selType"></param>
        /// <returns></returns>
        public static T GetModel<T>(this HttpRequestBase request,int selType=1) where T : new() {
            var type = typeof(T);
            if(type == null) return default(T);
            var proArrs = type.GetProperties();
            if(proArrs == null || proArrs.Count() <= 0) return default(T);
            T t = new T();
            foreach(var itemPro in proArrs) {
                string valueStr = string.Empty;
                if(selType==1) {
                    valueStr=request.GetQ(itemPro.Name);
                } else if(selType==2) {
                    valueStr = request.GetF(itemPro.Name);
                }else  if(selType==3){
                    valueStr = request.GetQ(itemPro.Name);
                    if(valueStr.IsNullOrWhiteSpace()) valueStr = request.GetF(itemPro.Name);
                }
                try {
                    var value = Convert.ChangeType(valueStr, itemPro.PropertyType);
                    itemPro.SetValue(t, value, null);
                } catch { }
            }
            return t;
        }
        #endregion

        #region "Helper Method"
        /// <summary>
        /// 根据Get请求是否有传callBack参数，返回跨域访问的callBack(..)格式字符串
        /// </summary>
        public static Func<string, string> GetCallBackStr = new Func<string, string>(delegate(string rtnStr) {
            string sellerInfoJsonCallBack = "{0}({1})";
            string callBack = HttpContext.Current.Request.QueryString["callback"];
            if(!string.IsNullOrWhiteSpace(callBack)) return string.Format(sellerInfoJsonCallBack, callBack, rtnStr);
            return rtnStr;
        });
        #endregion
    }


}
