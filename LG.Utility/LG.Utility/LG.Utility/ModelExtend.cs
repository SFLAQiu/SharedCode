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
                var cValue = row[itemPro.Name];
                if(cValue == null) continue;
                try {
                    object obj_val = Convert.ChangeType(cValue, itemPro.PropertyType);
                    itemPro.SetValue(t, obj_val, null);
                } catch { }
            }
            return t;
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
                if(cValue == null) continue;
                try {
                    object obj_val = Convert.ChangeType(cValue, itemPro.PropertyType);
                    itemPro.SetValue(t, obj_val, null);
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
       
    }
}