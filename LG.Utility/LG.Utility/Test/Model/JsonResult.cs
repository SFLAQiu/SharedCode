using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Model {
    [Serializable]
    class JsonResult {
        /// <summary>
        /// 返回值
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }

        ///// <summary>
        ///// 返回查询用户统计信息
        ///// </summary>
        //public Count uCount { get; set; }

        /// <summary>
        /// 回调方法名称
        /// </summary>
        public string Callback { get; set; }
    }
}
