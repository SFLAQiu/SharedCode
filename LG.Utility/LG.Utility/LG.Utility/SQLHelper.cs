using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Utility {
    public class SQLHelper {
        #region "辅助"
        /// <summary>
        /// 更具排序集合返回SQL
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static string GetOrderStr(List<OrderBy> orders) {
            StringBuilder orderSql = new StringBuilder();
            if (orders == null || orders.Count <= 0) return orderSql.ToString();
            bool isNeedComma = false;
            foreach (var item in orders) {
                if (isNeedComma) orderSql.Append(",");
                orderSql.Append(item.ToSql(false));
                isNeedComma = true;
            }
            return orderSql.ToString();
        }
        /// <summary>
        /// 获取数据行的SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public static string GetModelSQL(string tableName, string fields, string whereSql) {
            var sqlStr = @"
SELECT {0}
FROM {1}
WHERE {2}
".FormatStr(
    fields,
    tableName,
    whereSql
 );
            return sqlStr;
        }

        /// <summary>
        /// 获取删除的SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public static string GetDeleteSQL(string tableName, string whereSql) {
            var sqlStr = @"
DELETE FROM {0}
WHERE {1}
".FormatStr(
    tableName,
    whereSql
 );
            return sqlStr;
        }
        #endregion
    }
}
