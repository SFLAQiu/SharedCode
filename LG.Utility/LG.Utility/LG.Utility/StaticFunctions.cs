using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Collections;

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
                if(!Directory.Exists(fileFolder))
                    Directory.CreateDirectory(fileFolder);

                using(FileStream fs = new FileStream(filePath, FileMode.Create)) {
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
            if(!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            using(StreamWriter sw = new StreamWriter(filePath, append, charset)) {
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
            if(!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            using(StreamWriter sw = new StreamWriter(filePath, append, charset)) {
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
                using(FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
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
            if(!System.IO.File.Exists(filePath))
                return "";

            using(StreamReader sr = new StreamReader(filePath, charset)) {
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
            while(CacheEnum.MoveNext()) {
                HttpContext.Current.Cache.Remove(CacheEnum.Key.ToString());
            }
        }
        /// <summary>
        /// 移除一个缓存项目
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveServerCache(string key) {
            if(HttpContext.Current.Cache[key] != null) {
                HttpContext.Current.Cache.Remove(key);
            }
        }
    }
}
