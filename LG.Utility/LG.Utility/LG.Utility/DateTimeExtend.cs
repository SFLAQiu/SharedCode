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
        #endregion
    }
}
