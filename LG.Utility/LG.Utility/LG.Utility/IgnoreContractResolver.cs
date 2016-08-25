using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Utility {
    public class FilterContractResolver : DefaultContractResolver {
        private List<string> _Props { get; set; }
        private EFilterJson eFilter { get; set; }
        public FilterContractResolver(List<string> props=null, EFilterJson filter = EFilterJson.Include) {
            //属性的清单
            this._Props = props;
            this.eFilter = filter;
        }
        //重写创建要序列化的属性
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            list.ToList().ForEach((JsonProperty jItem) => {
                if (jItem.Ignored) jItem.Ignored = false;
            });
            //关闭OptOut 功能就直接返回所有
            if (eFilter == EFilterJson.CloseOptOut) return list;
            //自己来控制，包含哪些，和忽略哪些
            if (_Props == null || _Props.Count <= 0) return list;
            switch (eFilter) {
                case EFilterJson.Include: return list.Where(p => _Props.Contains(p.PropertyName)).ToList();
                case EFilterJson.Ignore: return list.Where(p => !_Props.Contains(p.PropertyName)).ToList();
            }
            return list;
        }
    }
    /// <summary>
    /// 过滤JSON 操作枚举
    /// </summary>
    public enum EFilterJson {
        /// <summary>
        /// 忽略
        /// </summary>
        Ignore=1,
        /// <summary>
        /// 包含
        /// </summary>
        Include=2,
        /// <summary>
        /// 关闭OptOut功能
        /// </summary>
        CloseOptOut = 3
    }
}
