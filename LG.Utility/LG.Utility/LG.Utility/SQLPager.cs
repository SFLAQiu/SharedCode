using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace LG.Utility {
    /// <summary>
    /// 排序的字段
    /// </summary>
    public class OrderBy {
        /// <summary>
        /// 排序的字段名,如果多表,字段名中需要包含表名部分
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表（别）名(为空则不输出)
        /// </summary>
        public string TableAliasName { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAsc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OrderBy() {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">字段中文名</param>
        /// <param name="isAsc"></param>
        public OrderBy(string name, bool isAsc) {
            Name = name;
            IsAsc = isAsc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableAliasName">表（别）名</param>
        /// <param name="name">字段中文名</param>
        /// <param name="isAsc"></param>
        public OrderBy(string tableAliasName, string name, bool isAsc) {
            TableAliasName = tableAliasName;
            Name = name;
            IsAsc = isAsc;
        }

        /// <summary>
        /// 输出sql语句段
        /// </summary>
        /// <param name="reIsAsc">是否相反当前的排序方式</param>
        /// <returns></returns>
        public string ToSql(bool reIsAsc) {
            var tmpIsAsc = reIsAsc ? !IsAsc : IsAsc;
            return (TableAliasName.IsNullOrWhiteSpace() ? "" : TableAliasName + ".") + Name + (tmpIsAsc ? " asc" : " desc");
        }
    }
    /// <summary>
    /// 排序的字段拓展
    /// </summary>
    public static class OrderByExtend {
        /// <summary>
        /// 排序枚举快速获得排序对象
        /// </summary>
        /// <param name="enumObj"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public static OrderBy ToOrderBy(this Enum enumObj, bool isAsc) {
            return new OrderBy(enumObj.ToString(), isAsc);
        }
        /// <summary>
        /// 排序枚举快速获得排序对象
        /// </summary>
        /// <param name="enumObj"></param>
        /// <param name="tableAliasName">表（别）名</param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public static OrderBy ToOrderBy(this Enum enumObj, string tableAliasName, bool isAsc) {
            return new OrderBy(tableAliasName, enumObj.ToString(), isAsc);
        }
        /// <summary>
        /// 联合其他OrderBy对象,得到OrderBy数组
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="unionOrderBy"></param>
        /// <returns></returns>
        public static OrderBy[] Union(this OrderBy orderBy, OrderBy unionOrderBy) {
            return new OrderBy[] { orderBy, unionOrderBy };
        }
        /// <summary>
        /// 联合其他OrderBy对象,得到OrderBy数组
        /// </summary>
        /// <param name="orderByArr"></param>
        /// <param name="unionOrderBy"></param>
        /// <returns></returns>
        public static OrderBy[] Union(this OrderBy[] orderByArr, OrderBy unionOrderBy) {
            return orderByArr.Concat(new OrderBy[] { unionOrderBy }).ToArray();
        }
        /// <summary>
        /// 联合其他OrderBy对象数组,得到OrderBy数组
        /// </summary>
        /// <param name="orderByArr"></param>
        /// <param name="unionOrderByArr"></param>
        /// <returns></returns>
        public static OrderBy[] Union(this OrderBy[] orderByArr, OrderBy[] unionOrderByArr) {
            return orderByArr.Concat(unionOrderByArr).ToArray();
        }
        /// <summary>
        /// 联合其他OrderBy对象数组,得到OrderBy数组
        /// </summary>
        /// <param name="orderByArr"></param>
        /// <param name="unionOrderByArr"></param>
        /// <returns></returns>
        public static OrderBy[] Union(this OrderBy orderByArr, OrderBy[] unionOrderByArr) {
            return new OrderBy[] { orderByArr }.Concat(unionOrderByArr).ToArray();
        }
    }

    /// <summary>
    /// v2.0
    /// </summary>
    public class SQLPager {
        /// <summary>
        /// 
        /// </summary>
        protected string _tableName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected string _aliasTName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected string _primaryKey = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected string _fromSqlApp = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected string _whereSql = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected string _orderBySql = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected Regex _replaceRx = new System.Text.RegularExpressions.Regex("", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tableName">单纯的主表名</param>
        /// <param name="aliasTName">如果有多表连接,那么该项就作为别名,无可为空</param>
        /// <param name="primaryKey">主表主键名</param>
        /// <param name="fromSqlApp">多表连接的语句段,将放在"from tableName 别名 "的后方,无可为空</param>
        /// <param name="whereSql">条件语句,无需where关键字</param>
        public SQLPager(string tableName, string aliasTName, string primaryKey, string fromSqlApp, string whereSql) {
            _tableName = (string.IsNullOrEmpty(tableName) ? "" : tableName).Trim();
            _aliasTName = (string.IsNullOrEmpty(aliasTName) ? "" : aliasTName).Trim();
            _primaryKey = (string.IsNullOrEmpty(primaryKey) ? "" : primaryKey).Trim();
            _fromSqlApp = (string.IsNullOrEmpty(fromSqlApp) ? "" : fromSqlApp).Trim();
            _whereSql = (string.IsNullOrEmpty(whereSql) ? "" : whereSql).Trim();

            if (string.IsNullOrEmpty(_aliasTName)) _aliasTName = _tableName;

            //防止多余的where关键字
            Regex whereRx = new Regex("^where", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _whereSql = whereRx.Replace(_whereSql, "");
        }

        /// <summary>
        /// 获得总页数的SQL语句
        /// </summary>
        /// <returns></returns>
        public string GetCountSQL() {
            return @"
select count(*) from [" + _tableName + @"] " + _aliasTName + @"
" + _fromSqlApp + @"
" + (string.IsNullOrEmpty(_whereSql) ? "" : "where " + _whereSql) + @"
";
        }

        /// <summary>
        /// 获得数据总和的SQL语句
        /// </summary>
        /// <returns></returns>
        public string GetSumSQL(string RMB) {
            return @"
select sum(" + RMB + ") from [" + _tableName + @"] " + _aliasTName + @"
" + _fromSqlApp + @"
" + (string.IsNullOrEmpty(_whereSql) ? "" : "where " + _whereSql) + @"
";
        }

        
        /// <summary>
        /// 获得分页的SQL语句(排序比较复杂的情况)
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="dataCount">总数</param>
        /// <param name="selectColumns">要select的字段列表</param>
        /// <param name="orderBySql">排序语句块,多表连接时,支持多表多字段排序</param>
        /// <returns></returns>
        public string GetPagerSql(int pageSize, int pageIndex, long dataCount, string selectColumns, params OrderBy[] orderBySql) {
            return GetPagerSql(pageSize, pageIndex, dataCount, selectColumns, orderBySql, true);
        }
        /// <summary>
        /// 获得分页的SQL语句(排序比较复杂的情况)
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="dataCount">总数</param>
        /// <param name="selectColumns">要select的字段列表</param>
        /// <param name="orderBySql">排序语句块,多表连接时,支持多表多字段排序</param>
        /// <param name="hasLastOrder">按这个排序分完页后的列表,是否还需要再按这个再次排序,一般是true,但是如果要把生成的语句再次作为一个子查询的sql,要为false</param>
        /// <returns></returns>
        public string GetPagerSql(int pageSize, int pageIndex, long dataCount, string selectColumns, OrderBy[] orderBySql, bool hasLastOrder) {
            return GetPagerSql(pageSize, pageIndex, dataCount, selectColumns, hasLastOrder, orderBySql);
        }
        /// <summary>
        /// 获得分页的SQL语句(排序比较复杂的情况)
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="dataCount">总数</param>
        /// <param name="selectColumns">要select的字段列表</param>
        /// <param name="hasLastOrder">按这个排序分完页后的列表,是否还需要再按这个再次排序,一般是true,但是如果要把生成的语句再次作为一个子查询的sql,要为false</param>
        /// <param name="orderBySql">排序语句块,多表连接时,支持多表多字段排序</param>
        /// <returns></returns>
        public string GetPagerSql(int pageSize, int pageIndex, long dataCount, string selectColumns, bool hasLastOrder, params OrderBy[] orderBySql) {

            _orderBySql = "";
            string waiOrderBy = "";
            if (orderBySql == null) orderBySql = new OrderBy[0];
            foreach (var orderBy in orderBySql) {
                _orderBySql += (string.IsNullOrEmpty(_orderBySql) ? "" : ",") + _aliasTName + "." + orderBy.ToSql(false);
                waiOrderBy += (string.IsNullOrEmpty(waiOrderBy) ? "" : ",") + _aliasTName + "." + orderBy.ToSql(true);
            }
            //如果没有主键排序,那么加入
            Regex hasPriKeyRx = new Regex(@"[\s]*,[\s]*([\s]*" + _aliasTName + @"[\s]*\.[\s]*)?[\s]*[\[]?[\s]*" + _primaryKey + @"[\s]*[\]]?[\s]*", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (orderBySql.FirstOrDefault(o => hasPriKeyRx.IsMatch("," + o.Name)) == null) {
                _orderBySql += (string.IsNullOrEmpty(_orderBySql) ? "" : ",") + _aliasTName + ".[" + _primaryKey + "] desc";
                waiOrderBy += (string.IsNullOrEmpty(waiOrderBy) ? "" : ",") + _aliasTName + ".[" + _primaryKey + "]";
            }

            if (pageSize <= 0) pageSize = 1;
            if (pageIndex <= 0) pageIndex = 1;

            //计算最后一页的数据量
            int lastPageSize = (int)(dataCount % pageSize);
            if (lastPageSize == 0 && dataCount > 0) lastPageSize = pageSize;//如果有数据,且取余为0,那么最后一页刚好是分页的页大小
            int pageCount = (int)(lastPageSize == pageSize ? dataCount / pageSize : dataCount / pageSize + 1);//计算总页数
            //如果当前页大于总页数,需要设置为总页数
            if (pageIndex > pageCount) pageIndex = (int)pageCount;

            //计算两次top的数值
            long top1 = pageSize * pageIndex, top2 = pageSize;
            if (top1 > dataCount) top1 = dataCount;
            if (pageIndex >= pageCount) top2 = lastPageSize;


            return @"
select " + selectColumns + @"
from [" + _tableName + @"] " + _aliasTName + @"
" + _fromSqlApp + @"
where " + _aliasTName + @".[" + _primaryKey + @"] in(
    select top " + top2 + @" " + _aliasTName + @".[" + _primaryKey + @"]
    from [" + _tableName + @"] " + _aliasTName + @"
    " + _fromSqlApp + @"
    where " + _aliasTName + @".[" + _primaryKey + @"] in(
        select top " + top1 + @" " + _aliasTName + @".[" + _primaryKey + @"]
        from [" + _tableName + @"] " + _aliasTName + @"
        " + _fromSqlApp + @"
        " + (string.IsNullOrEmpty(_whereSql) ? "" : "where " + _whereSql) + @"
        order by " + _orderBySql + @"
    )
    order by " + waiOrderBy + @"
)
" + (hasLastOrder ? "order by " + _orderBySql + @"" : "") + @"
";
        }
        #region 否决的、落后的
        /// <summary>
        /// [否决的，“string orderBySql”已经被“OrderBy[] orderBySql”代替]获得分页的SQL语句
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="dataCount">总数</param>
        /// <param name="selectColumns">要select的字段列表</param>
        /// <param name="orderBySql">排序语句块,多表连接时,支持多表多字段排序</param>
        /// <param name="hasLastOrder">按这个排序分完页后的列表,是否还需要再按这个再次排序,一般是true,但是如果要把生成的语句再次作为一个子查询的sql,要为false</param>
        /// <returns></returns>
        public string GetPagerSql(int pageSize, int pageIndex, long dataCount, string selectColumns, string orderBySql, bool hasLastOrder) {
            _orderBySql = orderBySql.Trim();

            //防止多余的order by关键字
            Regex orderByRx = new Regex("^order[ ]+by", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _orderBySql = orderByRx.Replace(_orderBySql, "");
            //整理下,将逗号前后面的空格都去掉
            Regex orderByDouSpRepRx = new Regex(@"[ ]*\,[ ]*", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _orderBySql = orderByDouSpRepRx.Replace(_orderBySql, ",");
            //整理下,连续的多个空格替换成一个
            Regex orderBySpRepRx = new Regex(@"[ ]+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _orderBySql = orderBySpRepRx.Replace(_orderBySql, " ");
            //判断排序字段中是否有主键存在
            Regex hasPriKeyRx = new Regex(@"[\s]*,[\s]*([\s]*" + _aliasTName + @"[\s]*\.[\s]*)?[\s]*[\[]?[\s]*" + _primaryKey + @"[\s]*[\]]?[\s]*", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!hasPriKeyRx.IsMatch("," + _orderBySql)) {
                //需要在排序字段中加主键
                _orderBySql = _orderBySql + (string.IsNullOrEmpty(_orderBySql) ? "" : ",") + _aliasTName + ".[" + _primaryKey + "]";
            }

            if (pageSize <= 0) pageSize = 1;
            if (pageIndex <= 0) pageIndex = 1;

            //第二次select的order by语句段:转成倒序
            Regex repTemDescRx = new Regex(@" desc\,", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex repTemAscRx = new Regex(@"[ ]?(asc)?\,", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string waiOrderBy = repTemAscRx.Replace(
                repTemDescRx.Replace(_orderBySql + ",", "{{temp}}"), " desc,").Replace("{{temp}}", " asc,");
            waiOrderBy = waiOrderBy.Substring(0, waiOrderBy.Length - 1);

            //计算最后一页的数据量
            int lastPageSize = (int)(dataCount % pageSize);
            if (lastPageSize == 0 && dataCount > 0) lastPageSize = pageSize;//如果有数据,且取余为0,那么最后一页刚好是分页的页大小
            int pageCount = (int)(lastPageSize == pageSize ? dataCount / pageSize : dataCount / pageSize + 1);//计算总页数
            //如果当前页大于总页数,需要设置为总页数
            if (pageIndex > pageCount) pageIndex = (int)pageCount;

            //计算两次top的数值
            long top1 = pageSize * pageIndex, top2 = pageSize;
            if (top1 > dataCount) top1 = dataCount;
            if (pageIndex >= pageCount) top2 = lastPageSize;


            return @"
select " + selectColumns + @"
from [" + _tableName + @"] " + _aliasTName + @"
" + _fromSqlApp + @"
where " + _aliasTName + @".[" + _primaryKey + @"] in(
    select top " + top2 + @" " + _aliasTName + @".[" + _primaryKey + @"]
    from [" + _tableName + @"] " + _aliasTName + @"
    " + _fromSqlApp + @"
    where " + _aliasTName + @".[" + _primaryKey + @"] in(
        select top " + top1 + @" " + _aliasTName + @".[" + _primaryKey + @"]
        from [" + _tableName + @"] " + _aliasTName + @"
        " + _fromSqlApp + @"
        " + (string.IsNullOrEmpty(_whereSql) ? "" : "where " + _whereSql) + @"
        order by " + _orderBySql + @"
    )
    order by " + waiOrderBy + @"
)
" + (hasLastOrder ? "order by " + _orderBySql + @"" : "") + @"
";
        }
        /// <summary>
        /// [否决的，“string orderBySql”已经被“OrderBy[] orderBySql”代替]获得分页的SQL语句
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="dataCount">总数</param>
        /// <param name="selectColumns">要select的字段列表</param>
        /// <param name="orderBySql">排序语句块,多表连接时,支持多表多字段排序</param>
        /// <returns></returns>
        public string GetPagerSql(int pageSize, int pageIndex, long dataCount, string selectColumns, string orderBySql) {
            return GetPagerSql(pageSize, pageIndex, dataCount, selectColumns, orderBySql, true);
        }
        #endregion
    }
}
