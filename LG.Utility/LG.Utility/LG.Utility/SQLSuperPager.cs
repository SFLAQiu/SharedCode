using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Utility {
    /// <summary>
    /// 超级SQL分页 v2.0
    /// </summary>
    public class SQLSuperPager : SQLPager {
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tableName">单纯的主表名</param>
        /// <param name="aliasTName">如果有多表连接,那么该项就作为别名,无可为空</param>
        /// <param name="primaryKey">主表主键名</param>
        /// <param name="fromSqlApp">多表连接的语句段,将放在"from tableName 别名 "的后方,无可为空</param>
        /// <param name="whereSql">条件语句,无需where关键字</param>
        public SQLSuperPager(string tableName, string aliasTName, string primaryKey, string fromSqlApp, string whereSql)
            : base(tableName, aliasTName, primaryKey, fromSqlApp, whereSql) {
        }
    }
}
