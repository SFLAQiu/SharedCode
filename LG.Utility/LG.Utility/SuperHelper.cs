using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;

namespace LG.Utility {
    public class TopSdkHelper {
        /// <summary>
        /// 获取请求响应对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="serverUrl"></param>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static T GetResponse<T>(ITopRequest<T> request, string serverUrl = "", string appKey = "", string appSecret = "") where T : TopResponse {
            serverUrl = string.IsNullOrWhiteSpace(serverUrl) ? "http://gw.api.taobao.com/router/rest" : serverUrl;
            appKey = string.IsNullOrWhiteSpace(appKey) ? "12011627" : appKey;
            appSecret = string.IsNullOrWhiteSpace(appSecret) ? "d63fc6135b1f956116d01969d6295bd0" : appSecret;
            ITopClient client = new DefaultTopClient(serverUrl, appKey, appSecret);
            var response = client.Execute(request);
            if (response != null && !response.IsError) return response;
            return null;
        }
    }
}
