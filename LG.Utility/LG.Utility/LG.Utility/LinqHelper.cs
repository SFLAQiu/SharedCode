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
        /// <param name="dataList">实现IEnumerable</param>
        /// <param name="whereHandle">delegate检索条件</param>
        /// <param name="orderHandle">delegate排序</param>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="dataCout">返回总数量</param>
        /// <param name="fpageSize">第一页数量,后面每页数量=pagesize</param>
        /// 
        /// <returns></returns>
        public static IEnumerable<T> GetIenumberable<T>(IEnumerable<T> dataList, Func<T, bool> whereHandle, Func<IEnumerable<T>, IEnumerable<T>> orderHandle, int pageSize, int pageIndex, out int dataCout, int fpageSize = 0) {
            if (pageIndex <= 0) pageIndex = 1;
            var getList = dataList;
            //查询操作
            if (whereHandle != null) getList = getList.Where(whereHandle);
            //排序操作
            if (orderHandle != null) getList = orderHandle.Invoke(getList);

            int skipNum = (pageIndex - 1) * pageSize;//过滤数量
            int takeNum = fpageSize > 0 ? (pageIndex == 1 ? fpageSize : pageSize) : pageSize;//截取数量

            if (fpageSize > 0 && pageIndex > 1) skipNum = skipNum + fpageSize - pageSize;
            //返回总数
            dataCout = 0;
            dataCout = (getList != null ? getList.Count() : 0);
            //分页
            getList = getList.Skip(skipNum).Take(takeNum);
            return getList;
        }
    }
}
