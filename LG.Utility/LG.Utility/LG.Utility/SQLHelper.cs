using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        /// 查询条件
        /// </summary>
        /// <typeparam name="T">查询条件类</typeparam>
        /// <param name="obj">查询条件对象</param>
        /// <param name="parames">返回SQL参数集合</param>
        /// <returns></returns>
        public static string GetWhere<T>(T obj, out List<SqlParameter> parames, string append = "", EWhereRelation appendRelation = EWhereRelation.AND) where T : class, new() {
            parames = new List<SqlParameter>();
            if (obj == null) return string.Empty;
            var searchType = typeof(T);
            var props = searchType.GetProperties();
            if (props == null || props.Count() <= 0) return string.Empty;
            //获取查询条件
            parames = GetSqlParameters(obj);
            //获取WHERE SQL语句
            var whereSql = GetWhereSql(props, obj);
            //附加查询条件
            if (append.IsNullOrWhiteSpace()) return whereSql;
            if (whereSql.IsNullOrWhiteSpace()) return append.GetString("");
            return $"{append.GetString("")} {appendRelation.ToString()} {whereSql}";
        }
        /// <summary>
        /// 获取对象属性作为SQL参数
        /// </summary>
        /// <param name="obj">参数对象</param>
        /// <returns>参数集合</returns>
        public static List<SqlParameter> GetSqlParameters(object obj) {
            var parames = new List<SqlParameter>();
            if (obj == null) return parames;
            var props = obj.GetType().GetProperties();
            if (props == null || props.Count() <= 0) return parames;
            foreach (var p in props) {
                var value = p.GetValue(obj, null);
                if (value == null) continue;
                //获取属性类型 DbType
                var pType = p.PropertyType;
                DbType dbType = DbType.Object;
                //空字符串不添加参数
                if (dbType == DbType.String && value.GetString().IsNullOrWhiteSpace()) continue;
                var isNullable = pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>);
                //泛型目前就支持 Nullable 类型可以添加参数
                if (pType.IsGenericType && !isNullable) continue;
                if (isNullable) {
                    dbType = pType.GetGenericArguments()[0].ToDbType();
                } else {
                    dbType = pType.ToDbType();
                }
                //sql参数添加
                parames.Add(new SqlParameter($"@{p.Name}", value) {
                    DbType = dbType
                });
            }
            return parames;
        }

        /// <summary>
        /// 拼接Where条件
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="fileName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private static string GetWhereSql(PropertyInfo[] props, object obj) {
            if (props == null || obj == null) return string.Empty;
            var joinIndex = 0;
            EWhereRelation lastRelation = EWhereRelation.AND;
            var whereSql = new StringBuilder();
            foreach (var p in props) {

                var pType = p.PropertyType;
                var dbType = pType.ToDbType();
                var value = p.GetValue(obj, null);
                if (value == null) continue;
                var isList = pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(List<>);
                var isNullable = pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>);
                //获取属性上的特性（Attribute）
                var attObj = p.GetCustomAttributes(typeof(SqlParameterAttribute), true).FirstOrDefault();
                if (attObj == null) continue;
                var attr = (SqlParameterAttribute)attObj;
                if (attr == null) continue;
                if (pType.IsGenericType && !isList && !isNullable) continue;


                //拼接条件关系 AND/OR
                if (joinIndex >= 1) {
                    //如果第一个查询条件是OR，那第二拼接无论是AND还是OR 都应该是OR
                    if (joinIndex == 1 && lastRelation == EWhereRelation.OR) attr.ERelation = EWhereRelation.OR;
                    whereSql.Append($" {attr.ERelation.ToString()} ");
                }
                var fileName = p.Name;
                if (!attr.FieldName.IsNullOrWhiteSpace()) fileName = attr.FieldName;

                //获取查询条件 Value 默认是变量@属性名，如果是List 是拼接字符串(1,32,66) 并且 运算符 会用 IN
                var val = $"@{p.Name}";
                if (isList) {
                    attr.EOperator = EWhereOperator.IN;
                    var listItemType = pType.GetGenericArguments()[0];
                    if (listItemType == null) return string.Empty;
                    if (listItemType == typeof(int)) {
                        val = $"({string.Join(",", value as List<int>)})";
                    } else if (listItemType == typeof(string)) {
                        val = $"({string.Join(",", value as List<string>)})";
                    } else {
                        continue;
                    }
                }

                var tableAlias = attr.TableAlias.IsNullOrWhiteSpace() ? "" : $"{attr.TableAlias}.";
                //拼接条件
                whereSql.Append($" {tableAlias}{fileName} {attr.EOperator.GetEnumAttr().Desc} {val}");
                //后续
                joinIndex++;
            }
            return whereSql.ToString();
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
