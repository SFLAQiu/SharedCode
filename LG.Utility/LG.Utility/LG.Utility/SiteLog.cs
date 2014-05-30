using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace LG.Utility {
    /// <summary>
    /// 站点日志输出类
    /// </summary>
    public class SiteLog {
        private static Log _log = new Log(HttpContext.Current.Server.MapPath("~/_______SiteLog/"));
        /// <summary>
        /// 输出日志字符串
        /// </summary>
        /// <param name="vlog"></param>
        /// <param name="dirName"></param>
        /// <param name="useIpDir"></param>
        public static void LogStr(string vlog, string dirName, bool useIpDir) {
            LogStr(vlog, dirName, useIpDir, false, false);
        }
        /// <summary>
        /// 输出日志字符串
        /// </summary>
        /// <param name="vlog"></param>
        /// <param name="dirName"></param>
        /// <param name="useIpDir"></param>
        /// <param name="useMonthDir"></param>
        /// <param name="useHourFileName"></param>
        public static void LogStr(string vlog, string dirName, bool useIpDir, bool useMonthDir, bool useHourFileName) {
            _log.LogStr(vlog, dirName, useIpDir, useMonthDir, useHourFileName);
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="vlog">日志内容</param>
        /// <param name="dirName">目录名称</param>
        public static void LogStr(string vlog, string dirName) {
            LogStr(vlog, dirName, false, true, false);
        }
    }
    /// <summary>
    /// 基础日志输出类
    /// </summary>
    public class Log {

        private object lockObj = new object();

        private string _rootDir;
        /// <summary>
        /// 用日志根目录初始化
        /// </summary>
        /// <param name="rootDir"></param>
        public Log(string rootDir) {
            _rootDir = new DirectoryInfo(rootDir).FullName + "\\";
        }
        private string GetDirName(string name) {
            return name.Replace(".", "_").Replace("/", "\\");
        }
        private string GetDateDir(bool useHourFileName) {
            return DateTime.Now.ToString("yyyy_MM_dd" + (useHourFileName ? "_HH" : ""));
        }
        /// <summary>
        /// 写日志字符串
        /// </summary>
        /// <param name="vlog"></param>
        /// <param name="dirName"></param>
        /// <param name="useIpDir"></param>
        /// <param name="useMonthDir"></param>
        /// <param name="useHourFileName"></param>
        public void LogStr(string vlog, string dirName, bool useIpDir, bool useMonthDir, bool useHourFileName) {
            DateTime now = DateTime.Now;

            string ip = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : string.Empty;
            if(ip.IsNullOrWhiteSpace()) ip = string.Empty;
            string ipDir = ip.Replace(":", "_").Replace(".", "_");
            string fileName = _rootDir
                + (dirName.IsNullOrWhiteSpace() ? "" : "\\" + GetDirName(dirName))
                + (useIpDir ? "\\" + ipDir : "")
                + (useMonthDir ? "\\" + now.Year + "_" + now.Month.ToString("00") : "")
                + "\\" + GetDateDir(useHourFileName) + ".txt";
            lock (lockObj) {
                StaticFunctions.WriteStringToFile(fileName, "[" + (useIpDir ? "" : (!ip.IsNullOrWhiteSpace() ? "IP:" + ip + "," : "")) + "Date:" + DateTime.Now.ToString() + "]" + vlog + "\r\n", true, System.Text.Encoding.Default);
            }
        }
    }
}
