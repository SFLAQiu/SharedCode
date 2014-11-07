using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace LG.Utility {
    /// <summary>
    /// 实体类拓展
    /// </summary>
    public static class ModelExtend {
        /// <summary>
        /// 根据DataTable 对象，绑定DataTable表默认首行数据值绑定到实体类属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T GetModel<T>(this DataTable dt, int rowNum = 0) where T : new() {
            if(dt == null || dt.Rows.Count <= 0) return default(T);
            var proArrs = typeof(T).GetProperties();
            if(proArrs == null || proArrs.Count() <= 0) return default(T);
            var row = dt.Rows[rowNum];
            T t = new T();
            foreach(var itemPro in proArrs) {
                if(!dt.Columns.Contains(itemPro.Name)) continue;
                if (!itemPro.CanWrite) continue;
                var cValue = row[itemPro.Name];
                if(cValue == null) continue;
                try {
                    var pType = GetPropertyType(itemPro.PropertyType);
                    if (pType.IsEnum) {
                        itemPro.SetValue(t, Enum.Parse(pType, cValue.ToString().Trim(), true), null);
                    } else {
                        itemPro.SetValue(t, Convert.ChangeType(cValue, itemPro.PropertyType), null);
                    }
                } catch { }
            }
            return t;
        }
        public static T GetModel<T>(this DataSet ds, int rowNum = 0) where T : new() {
            if (ds == null || ds.Tables[0] == null) return default(T);
            return ds.Tables[0].GetModel<T>();
        }
        /// <summary>
        /// 根据IDataReader 对象，绑定DataReader行数据值绑定到实体类属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T GetModelByIDataReader<T>(this IDataReader dr) where T : new() {
            if (dr == null) return default(T);
            var proArrs = typeof(T).GetProperties();
            if(proArrs == null || proArrs.Count() <= 0) return default(T);
            var drNames = new List<string>();
            for (int i = 0; i < dr.FieldCount; i++) drNames.Add(dr.GetName(i));
            T t = new T();
            foreach(var itemPro in proArrs) {
                if (!drNames.Exists(d => d == itemPro.Name)) continue;
                var cValue = dr[itemPro.Name];
                if (!itemPro.CanWrite) continue;
                if(cValue == null) continue;
                try {
                    var pType = GetPropertyType(itemPro.PropertyType);
                    if (pType.IsEnum) {
                        itemPro.SetValue(t, Enum.Parse(pType, cValue.ToString().Trim(), true), null);
                    } else {
                        itemPro.SetValue(t, Convert.ChangeType(cValue, itemPro.PropertyType), null);
                    }
                } catch { }
            }
            return t;
        }
        /// <summary>
        /// 根据IDataReader 对象，绑定DataReader行数据值绑定到实体类属性值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T GetModel<T>(this IDataReader dr) where T : new() {
            if (dr == null) return default(T);
            while (dr.Read()) {
                return dr.GetModelByIDataReader<T>();
            }
            return default(T);
        }
        /// <summary>
        /// 根据IDataReader 对象，绑定DataReader行数据值绑定到实体类属性值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static List<T> GetModels<T>(this IDataReader dr) where T : new() {
            List<T> lis = new List<T>();
            if(dr == null) return null;
            while(dr.Read()) {
                lis.Add(dr.GetModelByIDataReader<T>());
            }
            return lis;
        }
        /// <summary>
        /// 更具DataTable 对象，绑定DataTable表所有行值绑定到实体类属性值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> GetModels<T>(this DataTable dt) where T : new() {
            List<T> lis = new List<T>();
            if(dt == null || dt.Rows.Count <= 0) return null;
            for(int i = 0; i < dt.Rows.Count; i++) lis.Add(dt.GetModel<T>(rowNum:i));
            return lis;
        }
         /// <summary>
        /// 更具DataTable 对象，绑定DataTable表所有行值绑定到实体类属性值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> GetModels<T>(this DataSet ds) where T : new() {
            if (ds == null || ds.Tables[0] == null) return default(List<T>);
            return ds.Tables[0].GetModels<T>();
        }
        private static Type GetPropertyType(Type pType) {
            var gTypes = pType.GetGenericArguments();
            if (gTypes.Length > 0) return gTypes[0];
            return pType;
        }
    }
}