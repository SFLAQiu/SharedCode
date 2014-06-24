using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Model {
    [Serializable]
    public class ProductClickLog {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 商品点击数
        /// </summary>
        public int Count { get; set; }

    }
}
