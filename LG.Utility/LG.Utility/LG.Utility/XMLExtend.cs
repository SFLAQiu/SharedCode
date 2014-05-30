using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace LG.Utility {
    public static class XMLExtend {
        /// <summary>
        /// 更具xml节点获取节点中的值，如果为空时返回默认值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValue(this XElement ele, string defaultValue = "") {
            if (ele == null) return defaultValue;
            return ele.Value.GetString(defaultValue);
        }
        /// <summary>
        /// 更具xml节点属性获取属性中的值，如果为空时返回默认值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValue(this XAttribute at, string defaultValue = "") {
            if (at == null) return defaultValue;
            return at.Value.GetString(defaultValue);
        }
    }
}
