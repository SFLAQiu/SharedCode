using LG.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Test.Model;

namespace Test {
    class DYiYuanRelation {

         Database db = DatabaseFactory.CreateDatabase("fh_auxiliary");
        public List<YiYuanRelation> GetListV2(YiYuanRelationSearch search, int top = 0, params OrderBy[] orders) {
            List<SqlParameter> parames;
            var whereSql = SQLHelper.GetWhere<YiYuanRelationSearch>(search, out parames);
            var sqlStr = @"
SELECT {0} UserId,UserName,IOuterUserId,IOuterUserName,IOuterUserHeadUrl,IUserId,IPhoneNum,ActId,RegDate
FROM dbo.Act_YiYuanGou_Relation
{1}
{2}
".FormatStr(
     (top > 0 ? " top {0} ".FormatStr(top) : ""),
     (whereSql.IsNullOrWhiteSpace() ? "" : " WHERE {0} ".FormatStr(whereSql)),
     (orders.Length <= 0 ? "" : " ORDER BY {0} ".FormatStr(SQLHelper.GetOrderStr(orders.ToList())))
 );
            System.Diagnostics.Debug.WriteLine(sqlStr);
            System.Diagnostics.Debug.WriteLine(parames.Count());
            using (var cmd=db.GetSqlStringCommand(sqlStr)) {
                cmd.Parameters.AddRange(parames.ToArray());
                return db.ExecuteDataSet(cmd).GetModels<YiYuanRelation>();
            }
        }
        /// <summary>
        /// 获取一元购关系
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        //public YiYuanRelation GetModel(YiYuanRelationSearch search) {
        //    var datas = GetList(search, top: 1);
        //    return datas == null || datas.Count <= 0 ? null : datas[0];
        //}
        /// <summary>
        /// 获取where条件语句
        /// </summary>
        /// <param name="search"></param>
        /// <param name="parames"></param>
        /// <param name="isNeedAnd"></param>
        /// <returns></returns>
        //public string GetWhereSql(YiYuanRelationSearch search, out List<SqlParameter> parames, bool isNeedAnd = false) {
        //    var sql = new StringBuilder();
        //    parames = new List<SqlParameter>();
        //    if (!search.BothOrUserId.HasValue && search.UserId.HasValue) {
        //        if (isNeedAnd) sql.Append(" AND ");
        //        sql.Append(" UserId=@UserId ");
        //        if (!isNeedAnd) isNeedAnd = true;
        //        parames.Add(new SqlParameter("@UserId", search.UserId) {
        //            SqlDbType = SqlDbType.Int
        //        });
        //    }

        //    if (!search.BothOrUserId.HasValue && search.IUserId.HasValue) {
        //        if (isNeedAnd) sql.Append(" AND ");
        //        sql.Append(" IUserId=@IUserId ");
        //        if (!isNeedAnd) isNeedAnd = true;
        //        parames.Add(new SqlParameter("@IUserId", search.IUserId) {
        //            SqlDbType = SqlDbType.Int
        //        });
        //    }

        //    if (search.ActId.HasValue) {
        //        if (isNeedAnd) sql.Append(" AND ");
        //        sql.Append(" ActId=@ActId ");
        //        if (!isNeedAnd) isNeedAnd = true;
        //        parames.Add(new SqlParameter("@ActId", search.ActId) {
        //            SqlDbType = SqlDbType.Int
        //        });
        //    }

        //    if (search.BothOrUserId.HasValue) {
        //        if (isNeedAnd) sql.Append(" AND ");
        //        sql.Append(" (IUserId=@IUserId OR UserId=@UserId) ");
        //        if (!isNeedAnd) isNeedAnd = true;
        //        parames.Add(new SqlParameter("@IUserId", search.IUserId) {
        //            SqlDbType = SqlDbType.Int
        //        });
        //        parames.Add(new SqlParameter("@UserId", search.UserId) {
        //            SqlDbType = SqlDbType.Int
        //        });
        //    }

        //    if (!search.IPhoneNum.IsNullOrWhiteSpace()) {
        //        if (isNeedAnd) sql.Append(" AND ");
        //        sql.Append(" IPhoneNum=@IPhoneNum ");
        //        if (!isNeedAnd) isNeedAnd = true;
        //        parames.Add(new SqlParameter("@IPhoneNum", search.IPhoneNum) {
        //            SqlDbType = SqlDbType.NVarChar
        //        });
        //    }


        //    if (!search.IOuterUserId.IsNullOrWhiteSpace()) {
        //        if (isNeedAnd) sql.Append(" AND ");
        //        sql.Append(" IOuterUserId=@IOuterUserId ");
        //        if (!isNeedAnd) isNeedAnd = true;
        //        parames.Add(new SqlParameter("@IOuterUserId", search.IOuterUserId) {
        //            SqlDbType = SqlDbType.NVarChar
        //        });
        //    }

        //    if (search.IUserIsRegist.HasValue) {
        //        if (isNeedAnd) sql.Append(" AND ");
        //        if (search.IUserIsRegist.Value) {
        //            sql.Append(" IUserId>0 ");
        //        } else {
        //            sql.Append(" IUserId<=0 ");
        //        }
        //        if (!isNeedAnd) isNeedAnd = true;
        //    }

        //    return sql.ToString();
        //}
    }
}
