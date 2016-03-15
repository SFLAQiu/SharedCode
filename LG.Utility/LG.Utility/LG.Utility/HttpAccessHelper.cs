using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.Net;
using System.IO;
using System.Web;

namespace LG.Utility {
    /// <summary>
    /// HTTP请求的回调
    /// </summary>
    /// <param name="response">流读取器,如果请求出错,本值为null</param>
    /// <param name="responseCookies">返回新的cookie</param>
    /// <param name="err">如果请求有错误的话,返回该错误,否则为null</param>
    /// <param name="evData">自定义用的数据</param>
    public delegate void HttpAccessAsynCallBack(HttpWebResponse response, CookieCollection responseCookies, Exception err, Object evData);

    /// <summary>
    /// HTTP访问工具类
    /// </summary>
    public class HttpAccessHelper {



        #region GET请求
        private static HttpWebRequest GetHttpGetRequest(string url, Encoding dataEncoding, int timeoutMillisecond, CookieCollection requestCookies
             , string headerAccept, string headerReferer, string headerUserAgent) {
            HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            if (requestCookies != null) {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(requestCookies);
            }
            if (headerAccept.IsNotEmpty()) {
                request.Accept = headerAccept;
            }
            if (headerReferer.IsNotEmpty()) {
                request.Referer = headerReferer;
            }
            if (headerUserAgent.IsNotEmpty()) {
                request.UserAgent = headerUserAgent;
            }
            request.Timeout = timeoutMillisecond;
            request.Method = System.Net.WebRequestMethods.Http.Get;
            return request;
        }
        private static HttpWebRequest GetHttpGetRequest(string url, Encoding dataEncoding, int timeoutMillisecond, CookieCollection requestCookies) {
            return GetHttpGetRequest(url, dataEncoding, timeoutMillisecond, requestCookies, null, null, null);
        }

        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <param name="headerAccept">请求头字段:Accept</param>
        /// <param name="headerReferer">请求头字段:来源地址</param>
        /// <param name="headerUserAgent">请求头字段:客户端</param>
        /// <returns></returns>
        public static HttpWebResponse GetHttpGetResponse(string url, Encoding dataEncoding, int timeoutMillisecond, CookieCollection requestCookies, out CookieCollection responseCookies
            , string headerAccept, string headerReferer, string headerUserAgent) {

            HttpWebRequest request = GetHttpGetRequest(url, dataEncoding, timeoutMillisecond, requestCookies, headerAccept, headerReferer, headerUserAgent);

            //获取到响应流(需要等待)
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            responseCookies = GetCookiesByHeaderSetCookie(response);
            return response;
        }
        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <returns></returns>
        public static HttpWebResponse GetHttpGetResponse(string url, Encoding dataEncoding, int timeoutMillisecond, CookieCollection requestCookies, out CookieCollection responseCookies) {
            
            HttpWebRequest request = GetHttpGetRequest(url, dataEncoding, timeoutMillisecond, requestCookies);

            //获取到响应流(需要等待)
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            responseCookies = GetCookiesByHeaderSetCookie(response);
            return response;
        }

        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static HttpWebResponse GetHttpGetResponse(string url, Encoding dataEncoding, int timeoutMillisecond) {
            CookieCollection nullCookie = null;
            return GetHttpGetResponse(url, dataEncoding, timeoutMillisecond, null, out nullCookie);
        }


        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <param name="headerAccept">请求头字段:Accept</param>
        /// <param name="headerReferer">请求头字段:来源地址</param>
        /// <param name="headerUserAgent">请求头字段:客户端</param>
        /// <returns></returns>
        public static StreamReader GetHttpGetResponseStream(string url, Encoding dataEncoding, int timeoutMillisecond, CookieCollection requestCookies, out CookieCollection responseCookies
            , string headerAccept, string headerReferer, string headerUserAgent) {
                return new StreamReader(GetHttpGetResponse(url, dataEncoding, timeoutMillisecond, requestCookies, out responseCookies, headerAccept, headerReferer, headerUserAgent).GetResponseStream(), dataEncoding);
        }
        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <returns></returns>
        public static StreamReader GetHttpGetResponseStream(string url, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies, out CookieCollection responseCookies) {
            return new StreamReader(GetHttpGetResponse(url, dataEncoding, timeoutMillisecond, requestCookies, out responseCookies).GetResponseStream(), dataEncoding);
        }
        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static StreamReader GetHttpGetResponseStream(string url, Encoding dataEncoding, int timeoutMillisecond) {
            CookieCollection nullCookie = null;
            return GetHttpGetResponseStream(url, dataEncoding, timeoutMillisecond, null, out nullCookie);
        }

        /// <summary>
        /// Http模拟请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <param name="headerAccept">请求头字段:Accept</param>
        /// <param name="headerReferer">请求头字段:来源地址</param>
        /// <param name="headerUserAgent">请求头字段:客户端</param>
        /// <returns></returns>
        public static string GetHttpGetResponseText(string url, Encoding dataEncoding, int timeoutMillisecond, CookieCollection requestCookies, out CookieCollection responseCookies
            , string headerAccept, string headerReferer, string headerUserAgent) {

            var reader = GetHttpGetResponseStream(url, dataEncoding, timeoutMillisecond, requestCookies, out responseCookies, headerAccept, headerReferer, headerUserAgent);
            var text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

        /// <summary>
        /// Http模拟请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <returns></returns>
        public static string GetHttpGetResponseText(string url, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies, out CookieCollection responseCookies) {
            var reader = GetHttpGetResponseStream(url, dataEncoding, timeoutMillisecond, requestCookies, out responseCookies);
            var text = reader.ReadToEnd();
            reader.Close();
            return text;
        }
        /// <summary>
        /// Http模拟请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string GetHttpGetResponseText(string url, Encoding dataEncoding, int timeoutMillisecond) {
            CookieCollection nullCookie = null;
            return GetHttpGetResponseText(url, dataEncoding, timeoutMillisecond, null, out nullCookie);
        }

        /// <summary>
        /// 异步请求HTTP GET
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="invokeObject">回调函数的调用对象</param>
        /// <param name="callBackHandle">回调</param>
        /// <param name="evData">回调附加的传递参数</param>
        /// <returns></returns>
        public static HttpWebRequest AsynGet(string url, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies
            , System.ComponentModel.ISynchronizeInvoke invokeObject, HttpAccessAsynCallBack callBackHandle, Object evData) {
            HttpWebRequest request = GetHttpGetRequest(url, dataEncoding, timeoutMillisecond, requestCookies);

            System.Collections.ArrayList poD = new System.Collections.ArrayList();
            poD.Add(request);
            poD.Add(invokeObject);
            poD.Add(callBackHandle);

            System.Threading.WaitCallback edGetResponseCallB = new System.Threading.WaitCallback(delegate(object poData) {
                System.Collections.ArrayList pD = (System.Collections.ArrayList)poData;
                HttpWebRequest req = (HttpWebRequest)pD[0];
                System.ComponentModel.ISynchronizeInvoke invObj = (System.ComponentModel.ISynchronizeInvoke)pD[1];
                HttpAccessAsynCallBack callB = (HttpAccessAsynCallBack)pD[2];

                HttpWebResponse response = null;
                try {
                    response = (HttpWebResponse)req.GetResponse();
                } catch (Exception ex) {
                    //如果强制被取消的话,就抛错.所以错了就不调用了
                    invObj.Invoke(callB
                        , new Object[] { 
                        null
                        ,null
                        ,ex
                        ,evData
                    });
                    return;
                }
                invObj.Invoke(callB
                    , new Object[] { 
                        response
                        , GetCookiesByHeaderSetCookie(response)
                        ,null
                        ,evData
                    });
            });

            System.Threading.ThreadPool.QueueUserWorkItem(edGetResponseCallB, poD);

            return request;
        }

        #endregion

        #region POST请求

        /// <summary>
        /// POST的基类,获取HttpWebRequest
        /// </summary>
        private static HttpWebRequest GetHttpPostRequest(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies,string contentType= "multipart/form-data", string jsonStr="") {
            //分割符(字符串)
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            //分割符(二进制,用于最后的一个分隔符)
            byte[] boundaryBytes = dataEncoding.GetBytes("--" + boundary + "\r\n");
            //总请求大小
            long contentLength = 0;
            //
            byte[] tempByte;

            HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            if (requestCookies != null) {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(requestCookies);
            }
            request.Timeout = timeoutMillisecond;
            request.Method = System.Net.WebRequestMethods.Http.Post;
            request.ContentType =$"{contentType}; boundary=" + boundary;

            //获取到请求流
            System.IO.StreamWriter writer = new System.IO.StreamWriter(request.GetRequestStream());

            //***开始发送POST请求***

            //发送文本数据
            if (data != null) {
                foreach (string dataKey in data.Keys) {
                    tempByte = dataEncoding.GetBytes("--" + boundary + "\r\n"
                        + $"Content-Disposition: form-data; name=\"" + dataKey + "\"\r\n"
                        + "\r\n"
                        + data[dataKey]
                        + "\r\n");
                    contentLength += tempByte.Length;
                    writer.BaseStream.Write(tempByte, 0, tempByte.Length);
                }
            }

            //发送二进制数据
            if (byteData != null) {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象
                MemoryStream ms = new MemoryStream();//创建内存流对象
                foreach (string byteDataKey in byteData.Keys) {
                    tempByte = dataEncoding.GetBytes("--" + boundary + "\r\n"
                        + "Content-Disposition: form-data; name=\"" + byteDataKey + "\"; filename=\"" + (byteDataFileNames.ContainsKey(byteDataKey) ? byteDataFileNames[byteDataKey] : "") + "\"\r\n"
                        + "Content-Type: application/octet-stream\r\n"
                        + "\r\n");
                    contentLength += tempByte.Length;
                    writer.BaseStream.Write(tempByte, 0, tempByte.Length);

                    tempByte = byteData[byteDataKey];
                    contentLength += tempByte.Length;
                    //根据是否压缩,使用不同的方式写入二进制
                    if (isGZip) {
                        GZipStreamCompress(writer.BaseStream, tempByte);
                    } else {
                        writer.BaseStream.Write(tempByte, 0, tempByte.Length);
                    }
                    tempByte = dataEncoding.GetBytes("\r\n\r\n");
                    contentLength += tempByte.Length;
                    writer.BaseStream.Write(tempByte, 0, tempByte.Length);
                }
                ms.Close();//关闭内存流对象
                ms.Dispose();//释放资源
            }
            //最后一个分隔符
            if (contentLength > 0) {
                contentLength += boundaryBytes.Length;
                writer.BaseStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }
            //post JSON
            if (!jsonStr.IsNullOrWhiteSpace()) writer.Write(jsonStr);
            writer.Close();
            writer.Dispose();
            //***结束发送POST请求***

            return request;
        }

        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器,需要外部关闭
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">普通表单项(非二进制)</param>
        /// <param name="byteData">二进制数据(通常指附件)</param>
        /// <param name="byteDataFileNames">二进制数据对应的文件名(通常指附件)</param>
        /// <param name="isGZip">是否压缩数据(只针对二进制而言)</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static HttpWebResponse GetHttpPostResponse(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond) {
            CookieCollection nullCookie = null;
            return GetHttpPostResponse(url, data, byteData, byteDataFileNames, isGZip, dataEncoding, timeoutMillisecond, null, out nullCookie);
        }

        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器,带Cookies,需要外部关闭
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">普通表单项(非二进制)</param>
        /// <param name="byteData">二进制数据(通常指附件)</param>
        /// <param name="byteDataFileNames">二进制数据对应的文件名(通常指附件)</param>
        /// <param name="isGZip">是否压缩数据(只针对二进制而言)</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <returns></returns>
        public static HttpWebResponse GetHttpPostResponse(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies, out CookieCollection responseCookies) {
            HttpWebRequest request = GetHttpPostRequest(url, data, byteData, byteDataFileNames, isGZip, dataEncoding, timeoutMillisecond, requestCookies);
            //开始获取接收流
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            responseCookies = GetCookiesByHeaderSetCookie(response);

            return response;
        }
        public static string GetHttpPostJsonText(string url,Encoding dataEncoding,string jsonStr, CookieCollection requestCookies,out CookieCollection responseCookies, int timeoutMillisecond = 3000) {
            var request = GetHttpPostRequest(url, null, null, null, false, dataEncoding, timeoutMillisecond, requestCookies,contentType: "application/json",jsonStr: jsonStr);
            //开始获取接收流
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            responseCookies = GetCookiesByHeaderSetCookie(response);
            var reader = new StreamReader(response.GetResponseStream());
            string text = reader.ReadToEnd();
            reader.Close();
            return text;
        }
        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器,需要外部关闭
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">普通表单项(非二进制)</param>
        /// <param name="byteData">二进制数据(通常指附件)</param>
        /// <param name="byteDataFileNames">二进制数据对应的文件名(通常指附件)</param>
        /// <param name="isGZip">是否压缩数据(只针对二进制而言)</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static StreamReader GetHttpPostResponseStream(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond) {
            CookieCollection nullCookie = null;
            return GetHttpPostResponseStream(url, data, byteData, byteDataFileNames, isGZip, dataEncoding, timeoutMillisecond, null, out nullCookie);
        }

        /// <summary>
        /// Http模拟请求的基础实现方法,返回流读取器,带Cookies,需要外部关闭
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">普通表单项(非二进制)</param>
        /// <param name="byteData">二进制数据(通常指附件)</param>
        /// <param name="byteDataFileNames">二进制数据对应的文件名(通常指附件)</param>
        /// <param name="isGZip">是否压缩数据(只针对二进制而言)</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <returns></returns>
        public static StreamReader GetHttpPostResponseStream(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies, out CookieCollection responseCookies) {
            return new StreamReader(GetHttpPostResponse(url, data, byteData, byteDataFileNames, isGZip, dataEncoding, timeoutMillisecond
                , requestCookies, out responseCookies).GetResponseStream());
        }


        /// <summary>
        /// Http模拟请求的基础实现方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">普通表单项(非二进制)</param>
        /// <param name="byteData">二进制数据(通常指附件)</param>
        /// <param name="byteDataFileNames">二进制数据对应的文件名(通常指附件)</param>
        /// <param name="isGZip">是否压缩数据(只针对二进制而言)</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string GetHttpPostResponseText(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond) {
            CookieCollection nullCookie = null;
            return GetHttpPostResponseText(url, data, byteData, byteDataFileNames, isGZip, dataEncoding, timeoutMillisecond, null, out nullCookie);
        }

        /// <summary>
        /// Http模拟请求的基础实现方法,带Cookies
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">普通表单项(非二进制)</param>
        /// <param name="byteData">二进制数据(通常指附件)</param>
        /// <param name="byteDataFileNames">二进制数据对应的文件名(通常指附件)</param>
        /// <param name="isGZip">是否压缩数据(只针对二进制而言)</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="responseCookies">得到的cookie集合</param>
        /// <returns></returns>
        public static string GetHttpPostResponseText(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies, out CookieCollection responseCookies) {
            StreamReader reader = GetHttpPostResponseStream(url, data, byteData, byteDataFileNames, isGZip, dataEncoding, timeoutMillisecond
                , requestCookies, out responseCookies);
            string text = reader.ReadToEnd();
            reader.Close();
            return text;
        }



        /// <summary>
        /// 异步请求HTTP POST
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">普通表单项(非二进制)</param>
        /// <param name="byteData">二进制数据(通常指附件)</param>
        /// <param name="byteDataFileNames">二进制数据对应的文件名(通常指附件)</param>
        /// <param name="isGZip">是否压缩数据(只针对二进制而言)</param>
        /// <param name="dataEncoding">发送和读取数据的编码</param>
        /// <param name="timeoutMillisecond">超时时间(单位:毫秒)</param>
        /// <param name="requestCookies">提交的cookie集合</param>
        /// <param name="invokeObject">回调函数的调用对象</param>
        /// <param name="callBackHandle">回调</param>
        /// <param name="evData">回调附加的传递参数</param>
        /// <returns></returns>
        public static HttpWebRequest AsynPost(string url, IDictionary<string, string> data
            , IDictionary<string, byte[]> byteData, IDictionary<string, string> byteDataFileNames, bool isGZip, Encoding dataEncoding, int timeoutMillisecond
            , CookieCollection requestCookies
            , System.ComponentModel.ISynchronizeInvoke invokeObject, HttpAccessAsynCallBack callBackHandle, Object evData) {
            HttpWebRequest request = GetHttpPostRequest(url, data, byteData, byteDataFileNames, isGZip, dataEncoding, timeoutMillisecond, requestCookies);

            System.Collections.ArrayList poD = new System.Collections.ArrayList();
            poD.Add(request);
            poD.Add(invokeObject);
            poD.Add(callBackHandle);

            System.Threading.WaitCallback edGetResponseCallB = new System.Threading.WaitCallback(delegate(object poData) {
                System.Collections.ArrayList pD = (System.Collections.ArrayList)poData;
                HttpWebRequest req = (HttpWebRequest)pD[0];
                System.ComponentModel.ISynchronizeInvoke invObj = (System.ComponentModel.ISynchronizeInvoke)pD[1];
                HttpAccessAsynCallBack callB = (HttpAccessAsynCallBack)pD[2];

                HttpWebResponse response = null;
                try {
                    response = (HttpWebResponse)req.GetResponse();
                } catch (Exception ex) {
                    //如果强制被取消的话,就抛错.所以错了就不调用了
                    invObj.Invoke(callB
                        , new Object[] { 
                        null
                        ,null
                        ,ex
                        ,evData
                    });
                    return;
                }
                invObj.Invoke(callB
                    , new Object[] { 
                        response
                        , GetCookiesByHeaderSetCookie(response)
                        ,null
                        ,evData
                    });
            });

            System.Threading.ThreadPool.QueueUserWorkItem(edGetResponseCallB, poD);

            return request;
        }

        #endregion

        #region 压缩
        /// <summary>
        /// 压缩二进制并写入到流中,确保该流是打开状态的,写完后该流还是打开状态
        /// </summary>
        /// <param name="writeStream">压缩后的数据,写入该流</param>
        /// <param name="data">压缩二进制数据</param>
        /// <param name="leaveOpen">压缩等级</param>
        public static void GZipStreamCompress(Stream writeStream, Byte[] data, bool leaveOpen) {
            GZipStream gzipStream = new GZipStream(writeStream, CompressionMode.Compress, leaveOpen);//创建压缩对象
            gzipStream.Write(data, 0, data.Length);//把压缩后的数据写入文件
            gzipStream.Close();//关闭压缩流
            gzipStream.Dispose();//释放对象
        }
        /// <summary>
        /// 压缩二进制并写入到流中,确保该流是打开状态的,写完后该流还是打开状态
        /// </summary>
        /// <param name="writeStream">压缩后的数据,写入该流</param>
        /// <param name="data"></param>
        public static void GZipStreamCompress(Stream writeStream, Byte[] data) {
            GZipStreamCompress(writeStream, data, true);
        }
        #endregion

        /// <summary>
        /// 从响应中获取响应的cookie
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static CookieCollection GetCookiesByHeaderSetCookie(HttpWebResponse response) {
            CookieCollection cookies = response.Cookies;
            //如果已经自动取好,那么直接使用这个
            if (cookies != null && cookies.Count > 0) return cookies;

            /*
             * 否则下面就开始解析响应头里的cookie数据
             * name=value;p1=v;p2=v,name=value;p1=v;p2=v  ...
             * 其中过期时间的value中带有逗号,需要特殊判断
             */
            cookies = new CookieCollection();
            string headerSetCookie = response.Headers["Set-Cookie"].GetString(string.Empty);

            Cookie tmpCookie = new Cookie();
            tmpCookie.Domain = response.ResponseUri.DnsSafeHost;
            string tmpName = "";
            string tmpValue = "";
            bool inName = true;
            bool inValue = false;
            for (int i = 0; i < headerSetCookie.Length; i++) {
                var c = headerSetCookie[i];
                //单个属性完成
                bool proCom = false;
                //单个cookie完成
                bool cookieCom = false;
                switch (c) {
                    case '=':
                        inName = false;
                        inValue = true;
                        tmpValue = "";
                        break;
                    case ';':
                        proCom = true;
                        break;
                    case ',':
                        //遇到逗号需警惕,可能是时间里的逗号
                        if (!inName && inValue && tmpName.TrimEx().ToLower() == "expires" && tmpValue.TrimEx().Length < 5) {
                            tmpValue += c;
                        } else {
                            proCom = true;
                            cookieCom = true;
                        }
                        break;
                    default:
                        if (inName) {
                            tmpName += c;
                        } else if (inValue) {
                            tmpValue += c;
                        }
                        break;
                }
                //如果是最后一个字符
                if (i >= headerSetCookie.Length - 1) {
                    proCom = true;
                    cookieCom = true;
                }

                if (proCom) {
                    inName = true;
                    inValue = false;
                    tmpName = tmpName.TrimEx();
                    tmpValue = tmpValue.TrimEx();
                    if (tmpCookie.Name.IsEmpty()) {
                        tmpCookie.Name = tmpName;
                        tmpCookie.Value = tmpValue;
                    } else {
                        try {
                            switch (tmpName.ToLower()) {
                                case "domain":
                                    tmpCookie.Domain = tmpValue;
                                    break;
                                case "expires":
                                    tmpCookie.Expires = DateTime.Parse(tmpValue);
                                    //cookie.Expires = DateTime.ParseExact(proValue, "r", System.Globalization.CultureInfo.InvariantCulture);
                                    break;
                                case "path":
                                    tmpCookie.Path = tmpValue;
                                    break;
                                case "httponly":
                                    tmpCookie.HttpOnly = true;
                                    break;
                                case "secure":
                                    tmpCookie.Secure = true;
                                    break;
                                case "version":
                                    tmpCookie.Version = tmpValue.GetInt(0, false);
                                    break;
                            }
                        } catch { }
                    }
                    tmpName = "";
                    tmpValue = "";
                }
                if (cookieCom && tmpCookie != null && tmpCookie.Name.IsNotEmpty()) {
                    cookies.Add(tmpCookie);
                    tmpCookie = new Cookie();
                    tmpCookie.Domain = response.ResponseUri.DnsSafeHost;
                }
            }

            response.Cookies = cookies;
            return cookies;
        }
        #region "图片下载"
        /// <summary>
        /// 从图片地址下载图片到本地磁盘
        /// </summary>
        /// <param name="ToLocalPath">图片本地磁盘地址</param>
        /// <param name="url">图片网址</param>
        /// <returns></returns>
        public static bool SavePhotoFromUrl(string fileName, string url, out string errMsg) {
            errMsg = string.Empty;
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                response = request.GetResponse();
                stream = response.GetResponseStream();
                FileInfo fileInfo = new FileInfo(fileName);
                if (!Directory.Exists(fileInfo.DirectoryName)) Directory.CreateDirectory(fileInfo.DirectoryName);
                if (!response.ContentType.ToLower().StartsWith("text/")) Value = SaveBinaryFile(response, fileName);
            } catch (Exception err) {
                errMsg = err.ToString();
            }
            return Value;
        }
        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        private static bool SaveBinaryFile(WebResponse response, string FileName) {
            bool Value = true;
            byte[] buffer = new byte[1024];
            try {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();
                int l;
                do {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);
                outStream.Close();
                inStream.Close();
            } catch {
                Value = false;
            }
            return Value;
        }
        #endregion
        /// <summary>
        /// 获取发起请求的手机客户端类型
        /// </summary>
        /// <returns></returns>
        public static EMobilePhoneClient? GetRequestPhoneClient() {
            var userAgent = HttpContext.Current.Request.Headers["User-Agent"];
            if (userAgent.IsNullOrWhiteSpace()) return null;
            var elist = EnumHelper<EMobilePhoneClient>.GetAllItem();
            foreach (var item in elist) {
                if (userAgent.Contains(item.GetEnumAttr().Text)) return item;
            }
            return null;

        }
        
    }
    /// <summary>
    /// 移动手机客户端类型
    /// <para>
    /// EnumAttr Text=请求头中UserAgent 的信息，用来判断对应到客户端类型枚举
    /// </para>
    /// </summary>
    public enum EMobilePhoneClient {
        [EnumAttr(Text = "Android", Desc = "安坐")]
        Android=1,
        [EnumAttr(Text = "iPhone", Desc = "苹果")]
        IPhone=2,
        [EnumAttr(Text = "iPod", Desc = "苹果iPod")]
        IPod = 3,
        [EnumAttr(Text = "iPad", Desc = "苹果iPad")]
        IPad = 4,
        [EnumAttr(Text = "Windows Phone", Desc = "苹果iPad")]
        WindowsPhone=5
    }
}
