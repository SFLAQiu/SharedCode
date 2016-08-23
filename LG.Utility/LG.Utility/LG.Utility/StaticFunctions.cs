using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Collections;
using System.Net;
using System.Drawing;

namespace LG.Utility {
    public class StaticFunctions {
        /// <summary>
        /// 根据字节流生成文件
        /// </summary>
        /// <param name="filePath">目标文件路径</param>
        /// <param name="fileContent">文件内容</param>
        /// <returns></returns>
        public static bool CreateFile(string filePath, byte[] fileContent) {
            try {
                string fileFolder = System.IO.Path.GetDirectoryName(filePath);
                if (!Directory.Exists(fileFolder))
                    Directory.CreateDirectory(fileFolder);

                using (FileStream fs = new FileStream(filePath, FileMode.Create)) {
                    BinaryWriter br = new BinaryWriter(fs);
                    br.Write(fileContent);
                    br.Close();
                    fs.Close();
                    fs.Dispose();
                }
                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// 在当前流输出文件
        /// </summary>
        /// <param name="attachmentName">附件名称</param>
        /// <param name="Content">内容</param>
        /// <param name="encoding">编码</param>
        public static void OutPutFileInResponse(string attachmentName, byte[] Content, System.Text.Encoding encoding) {
            try {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("Content-Type", "application/octet-stream");
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(attachmentName));
                System.Web.HttpContext.Current.Response.ContentEncoding = encoding;
                System.Web.HttpContext.Current.Response.Charset = encoding.EncodingName;
                System.Web.HttpContext.Current.Response.BinaryWrite(Content);
                System.Web.HttpContext.Current.Response.End();
            } catch {

            }
        }



        /// <summary>
        /// 将字符串写入文件
        /// </summary>
        /// <param name="filePath">目标文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="append">是否追加</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static bool WriteStringToFile(string filePath, string content, bool append, System.Text.Encoding charset) {
            string folder = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            using (StreamWriter sw = new StreamWriter(filePath, append, charset)) {
                try {
                    sw.Write(content);
                    //写入换行符
                    sw.WriteLine();
                    return true;
                } catch {
                    throw;
                } finally {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }


        /// <summary>
        /// 将字符串写入文件，写失败时，不抛出异常
        /// </summary>
        /// <param name="filePath">目标文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="append">是否追加</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static bool WriteStringToFileDoNotThrowEx(string filePath, string content, bool append, System.Text.Encoding charset) {
            string folder = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            using (StreamWriter sw = new StreamWriter(filePath, append, charset)) {
                try {
                    sw.Write(content);
                    //写入换行符
                    sw.WriteLine();
                    return true;
                } catch {
                    return false;
                } finally {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        /// <summary>
        /// 获得文件二进制
        /// </summary>
        /// <param name="filePath">源文件路径</param>
        /// <returns></returns>
        public static byte[] GetFileByte(string filePath) {
            try {
                // 打开文件 
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    // 读取文件的 byte[] 
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, bytes.Length);
                    fileStream.Close();
                    fileStream.Dispose();
                    return bytes;
                }
            } catch {
                return null;
            }
        }


        /// <summary>
        /// 读取文件中的字符串
        /// </summary>
        /// <param name="filePath">源文件路径</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string GetFileString(string filePath, System.Text.Encoding charset) {
            if (!System.IO.File.Exists(filePath))
                return "";

            using (StreamReader sr = new StreamReader(filePath, charset)) {
                try {
                    return sr.ReadToEnd();
                } catch {
                    return "";
                } finally {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }

        /// <summary>
        /// 清空服务器缓存
        /// </summary>
        public static void ClearServerCache() {
            IDictionaryEnumerator CacheEnum = HttpContext.Current.Cache.GetEnumerator();
            while (CacheEnum.MoveNext()) {
                HttpContext.Current.Cache.Remove(CacheEnum.Key.ToString());
            }
        }
        /// <summary>
        /// 移除一个缓存项目
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveServerCache(string key) {
            if (HttpContext.Current.Cache[key] != null) {
                HttpContext.Current.Cache.Remove(key);
            }
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

        /// <summary>
        /// 字节转img
        /// </summary>
        /// <param name="byt"></param>
        /// <returns></returns>
        public static Image ByteToImg(byte[] byt) {
            MemoryStream ms = new MemoryStream(byt);
            Image img = Image.FromStream(ms);
            return img;
        }

        /// <summary>
        /// 根据URL保存图片
        /// </summary>
        /// <param name="imgUrl">图片URL地址</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="saveFileName">保存图片名，不包括拓展名</param>
        /// <returns></returns>
        public static bool SaveFile(string imgUrl, string savePath, string saveFileName, int width = 0, int height = 0) {
            if (imgUrl.IsNullOrWhiteSpace()) return false;
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            try {
                using (var webClient = new WebClient()) {
                    var uri = new Uri(imgUrl);
                    if (uri == null) return false;
                    var fileName = Path.GetFileName(imgUrl);
                    var extension = Path.GetExtension(imgUrl);
                    if (extension.Contains("?")) {
                        var eNum = extension.IndexOf("?");
                        extension = extension.Substring(0, eNum);
                    }
                    byte[] imgByte = webClient.DownloadData(imgUrl);
                    if (imgByte == null || imgByte.Count() <= 0) return false;
                    var ms = new MemoryStream(imgByte);
                    if (ms == null) return false;
                    string saveFilePath = savePath + saveFileName + extension;
                    var newImg = System.Drawing.Image.FromStream(ms);
                    if (newImg == null) return false;
                    if (width > 0 && newImg.Width > width) {
                        var newHeight = height;
                        var ratio = (double)(width / (newImg.Width * 1.0));
                        if (newHeight <= 0) newHeight = (ratio * newImg.Height).GetInt(0, false);
                        newImg = newImg.GetThumbnailImage(width, newHeight, null, IntPtr.Zero);
                    }
                    newImg.Save(saveFilePath);
                    webClient.Dispose();
                }
            } catch {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 尽最大努力取得用户IP（这个版本已经不再维护，请使用V2最新版本）
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
        /// 获取客户端IP地址
        /// </summary>
        public static string GetClientRealIpV2() {
            if (HttpContext.Current == null) return string.Empty;
            var userHostAddress = string.Empty;
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            if (!HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IsNullOrWhiteSpace())
                userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0].Trim();
            //否则直接读取REMOTE_ADDR获取客户端IP地址
            if (userHostAddress.IsNullOrWhiteSpace()) userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (userHostAddress.IsNullOrWhiteSpace()) userHostAddress = HttpContext.Current.Request.UserHostAddress;
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!userHostAddress.IsNullOrWhiteSpace() && userHostAddress.IsIP()) return userHostAddress;
            return string.Empty;
        }
        /// <summary>
        /// 输出客服端，下载文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="saveFileName"></param>
        public static void OutClientToDownFile(string filePath, string saveFileName) {
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("cache-control", "private");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + saveFileName);
            HttpContext.Current.Response.WriteFile(filePath);
        }
        /// <summary>
        /// 获取位状态
        /// </summary>
        /// <param name="str"></param>
        /// <param name="position">从1开始</param>
        /// <returns></returns>
        public static bool GetPosStatus(string str, int position) {
            if (str.IsNullOrWhiteSpace()) return false;
            return str.Substring(position - 1, 1).GetInt(0, false) > 0;
        }
        /// <summary>
        /// 修改位状态
        /// </summary>
        /// <param name="str"></param>
        /// <param name="position">从1开始</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string SetPosStatus(string str, int position, bool status) {
            if (str.IsNullOrWhiteSpace()) return str;
            var indexStatus = GetPosStatus(str, position).GetInt(0, false) > 0;
            if (indexStatus.Equals(status)) return str;
            StringBuilder sb = new StringBuilder(str);
            sb.Replace(indexStatus.GetInt(0, false).GetString(), status.GetInt(0, false).GetString(), position - 1, 1);
            return sb.GetString();
        }
        /// <summary>
        /// 修改位状态
        /// </summary>
        /// <param name="str"></param>
        /// <param name="position">从1开始</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static void SetPosStatus(ref string str, int position, bool status) => str = SetPosStatus(str, position, status);
        /// <summary>
        /// 获取位状态字符串初始化
        /// <para>0.... length=0的个数，默认=8</para>
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string InitPostStatus(int length = 8) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                sb.Append("0");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 多次执行方法
        /// </summary>
        /// <param name="doFn">委托方法</param>
        /// <param name="failureTimes">失败后重复执行的次数</param>
        /// <returns>返回最后执行的结果</returns>
        public static bool DoFnAgain(Func<bool> doFn, int failureTimes = 3) {
            var isSc = false;
            if (doFn == null || failureTimes <= 0) return isSc;
            for (int i = 0; i < failureTimes; i++) {
                isSc = doFn();
                if (isSc) break;
            }
            return isSc;
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="values">需要加密的值</param>
        /// <returns></returns>
        public static string GetSignatureBySha1(List<string> values)
        {
            var array = values.ToArray();
            Array.Sort(array, string.CompareOrdinal);
            var s = string.Join("", array);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Array = sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
            var result = new StringBuilder();
            foreach (var b in sha1Array)
            {
                result.AppendFormat("{0:x2}", b);
            }
            return result.ToString();
        }
    }
}
