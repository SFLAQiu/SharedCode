using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Utility {
    public class Rand {
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length) {
            StringBuilder result = new StringBuilder();
            System.Random random = new Random(StaticFunctions.GetRandSeed());
            for (int i = 0; i < Length; i++) {
                  result.Append(random.Next(10));
            }
            return result.ToString();
        }

        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Str(int Length) {
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder result = new StringBuilder();
            int n = Pattern.Length;
            System.Random random = new Random(StaticFunctions.GetRandSeed());
            for (int i = 0; i < Length; i++) {
                int rnd = random.Next(0, n);
                result.Append(Pattern[rnd]);
            }
            return result.ToString();
        }



        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Str_char(int Length) {
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder result = new StringBuilder();
            int n = Pattern.Length;
            System.Random random = new Random(StaticFunctions.GetRandSeed());
            for (int i = 0; i < Length; i++) {
                int rnd = random.Next(0, n);
                result.Append(Pattern[rnd]);
            }
            return result.ToString();
        }
    }
}
