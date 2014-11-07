using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Utility {
    /// <summary>
    /// 名值对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="N"></typeparam>
    public class NameValue<T,N> {
        /// <summary>
        /// 名
        /// </summary>
        public T Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public N Value { get; set; }
    }
}
