using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Utility {
    public static class LinqHelper {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List">实现IEnumerable</param>
        /// <param name="FunWhere">delegate检索条件</param>
        /// <param name="FunOrder">delegate排序</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <returns></returns>
        public static IEnumerable<T> GetIenumberable<T>(IEnumerable<T> dataList, Func<T, bool> FunWhere, Func<T, string> FunOrder, int PageSize, int PageIndex) {
            //var rance = List.Where(FunWhere).OrderByDescending(FunOrder).Select(t => t).Skip((PageIndex - 1) * PageSize).Take(PageSize);
            var getList = dataList;
            if (FunWhere != null) getList = getList.Where(FunWhere);
            if (FunOrder != null) getList = getList.OrderByDescending(FunOrder);
            getList = getList.Select(t => t).Skip((PageIndex - 1) * PageSize).Take(PageSize);
            return getList;
        }
    }
}
