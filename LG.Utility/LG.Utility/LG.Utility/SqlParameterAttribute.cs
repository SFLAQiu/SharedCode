using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
namespace LG.Utility {
    /// <summary>
    /// SQL参数特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlParameterAttribute : Attribute {
        /// <summary>
        /// 查询条件的操作符
        /// </summary>
        public EWhereOperator EOperator { get; set; } = EWhereOperator.Equal;
        /// <summary>
        /// 查询条件之间的关系
        /// </summary>
        public EWhereRelation ERelation { get; set; } = EWhereRelation.AND;
        /// <summary>
        /// 表别名(多表的时候使用别名区分查询字段是哪张表)
        /// </summary>
        public string TableAlias { get; set; } = "";
        /// <summary>
        /// 查询字段名(当一个字段需要多个查询条件的时候会使用到)
        /// <para>默认等于属性名</para>
        /// </summary>
        public string FieldName { get; set; } = "";
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; } = "";
    }
    /// <summary>
    /// 查询条件操作符
    /// </summary>
    public enum EWhereOperator {
        /// <summary>
        /// 等于 =
        /// </summary>
        [EnumAttr(Desc = "=")]
        Equal = 1,
        /// <summary>
        /// 不等于 !=
        /// </summary>
        [EnumAttr(Desc = "!=")]
        UnEqual = 2,
        /// <summary>
        /// 大于 >
        /// </summary>
        [EnumAttr(Desc = ">")]
        MoreThan = 3,
        /// <summary>
        /// 小于 <
        /// </summary>
        [EnumAttr(Desc = "<")]
        LessThan = 4,
        /// <summary>
        /// 大于等于 >=
        /// </summary>
        [EnumAttr(Desc = ">=")]
        MoreThanOrEqual = 5,
        /// <summary>
        /// 小于等于 <=
        /// </summary>
        [EnumAttr(Desc = "<=")]
        LessThanOrEqual = 6,
        /// <summary>
        /// 模糊查询
        /// </summary>
        [EnumAttr(Desc = "LIKE")]
        Like = 7,
        /// <summary>
        /// IN
        /// </summary>
        [EnumAttr(Desc = "IN")]
        IN =8,
        /// <summary>
        /// NOT IN
        /// </summary>
        [EnumAttr(Desc = "NOT IN")]
        NotIn = 9
    }
    /// <summary>
    /// 查询条件之间的关系
    /// </summary>
    public enum EWhereRelation {
        /// <summary>
        /// AND
        /// </summary>
        AND = 1,
        /// <summary>
        /// OR
        /// </summary>
        OR = 2
    }
}
