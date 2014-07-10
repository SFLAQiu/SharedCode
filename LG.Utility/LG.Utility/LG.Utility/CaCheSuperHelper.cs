using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace LG.Utility {
    public class CaCheSuperHelper {
        /// <summary>
        /// 3600 * 24
        /// </summary>
        private static long _maxSeconds = 3600 * 24;
        /// <summary>
        /// 获取指定的缓存
        /// </summary>
        /// <param name="key">Cache的名称</param>
        /// <returns></returns>
        public static object Get(string key) {
            if (string.IsNullOrEmpty(key)) return null;
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// 返回指定的缓存,如果不存在就用函数返回的值,自动加入缓存.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> fun, double seconds) {
            T t;
            object o = Get(key);
            if (o != null && o is T) {
                t = (T)o;
            } else {
                t = fun();
                Set(key, t, seconds);
            }
            return t;
        }

        /// <summary>
        /// 删除指定的缓存
        /// </summary>
        /// <param name="key">Cache的名称</param>
        public static void Remove(string key) {
            if (string.IsNullOrEmpty(key)) return;
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 设置Cache的值,过期时间:到时间绝对过期. 时间秒.
        /// </summary>
        /// <param name="key">Cache的名称</param>
        /// <param name="value">Cache的值</param>
        /// <param name="seconds">过期时间,相对当前时间: 秒. 默认:1800</param>        
        public static void Set(string key, object value, double seconds) {
            if (value == null || string.IsNullOrEmpty(key)) return;
            if (seconds < 1) seconds = _maxSeconds;
            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration);
        }
    }
}
