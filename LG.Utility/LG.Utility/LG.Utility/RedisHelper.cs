using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace LG.Utility {
    /// <summary>
    /// Redis 帮助类
    /// </summary>
    public class RedisHelper {
        private string _RedisConnection { get; set; }
        private string _RedisConnectionAuth { get; set; }
        public RedisHelper(string redisConnection, string redisConnectionAuth = "") {
            _RedisConnection = redisConnection;
            _RedisConnectionAuth = redisConnectionAuth;
        }
        private static PooledRedisClientManager _pcm;
        private  PooledRedisClientManager GetDefaultManager() {
            if (_pcm != null) return _pcm;
            _pcm = new PooledRedisClientManager(new[] { _RedisConnection });
            _pcm.ConnectTimeout = 1000;
            _pcm.IdleTimeOutSecs = 30;
            return _pcm;
        }
        /// <summary>
        /// 获取链接redis对象
        /// </summary>
        /// <returns></returns>
        public  IRedisClient GetClient() {
            IRedisClient client = GetDefaultManager().GetClient();
            if (!_RedisConnectionAuth.IsNullOrWhiteSpace()) client.Password = _RedisConnectionAuth;
            return client;
        }
        #region "帮助拓展"
        #region "字符串类型"
        /// <summary>
        /// 获取Redis存储类型为string 的数据 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(IRedisClient client,string key) where T : class {
            var isHave = client.ContainsKey(key);
            if (isHave) return client.Get<T>(key);
            return default(T);
        }
        /// <summary>
        /// 获取Redis存储类型为string 的数据 ,如果不存在就用函数返回的值,自动加入缓存.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public  T Get<T>(IRedisClient client,string key, Func<T> fun, DateTime expiresAt) where T : class {
            var chacheData = Get<T>(client,key);
            if (chacheData != null) return chacheData;
            var saveData = fun();
            client.Set<T>(key, saveData, expiresAt);
            return saveData;
        }
        /// <summary>
        /// 移除key对应的Redis缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(IRedisClient client, string key) {
            client.Remove(key);
        }
        #endregion
        #region "哈希类型"
        /// <summary>
        /// 获取hashvalue（值），根据key 和field(字段)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string GetHash(IRedisClient client, string key, string field) {
            var isHave = client.ContainsKey(key);
            if (!isHave) return string.Empty;
            return client.GetValueFromHash(key, field);
        }

        public bool SetHash(IRedisClient client,string key,string field,string value) {
            return client.SetEntryInHash(key, field, value);
        }
        #endregion
        #endregion
    }
}
