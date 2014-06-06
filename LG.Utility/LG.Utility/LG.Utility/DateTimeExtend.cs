using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Utility {
    public static class DateTimeExtend {
        #region 时间戳
        private static DateTime StandardDate = DateTime.Parse("1970-1-1 0:0:0");
        /// <summary>
        /// 计算时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetTimestamp(this DateTime date) {
            return (long)date.Subtract(StandardDate).TotalSeconds;
        }
        /// <summary>
        /// 根据时间戳(长整型)得到时间对象
        /// </summary>
        /// <param name="tick"></param>
        /// <returns></returns>
        public static DateTime ParseDateByTimestamp(this long tick) {
            return StandardDate.AddSeconds(tick);
        }
        /// <summary>
        /// 根据时间枚举转成中文,默认返回中文 (一，二，三....)
        /// </summary>
        /// <param name="eWeek">时间枚举</param>
        /// <param name="prefixStr">时间字符串前缀如："星期"+"一"</param>
        /// <returns></returns>
        public static string GetWeekCN(this DayOfWeek eWeek,string prefixStr="") {
            switch(eWeek) {
                case DayOfWeek.Monday: return prefixStr+"一";
                case DayOfWeek.Tuesday: return prefixStr + "二";
                case DayOfWeek.Wednesday: return prefixStr + "三";
                case DayOfWeek.Thursday: return prefixStr + "四";
                case DayOfWeek.Friday: return prefixStr + "五";
                case DayOfWeek.Saturday: return prefixStr + "六";
                case DayOfWeek.Sunday: return prefixStr + "七";
            }
            return "未知";
        }
        #endregion
    }
}
