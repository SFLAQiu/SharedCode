using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.UI.WebControls;

namespace LG.Utility {
    /// <summary>
    /// 枚举工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EnumHelper<T> {
        /// <summary>
        /// 获取所有枚举
        /// </summary>
        /// <returns></returns>
        public static List<T> GetAllItem() {
            List<T> list = new List<T>();
            Type enType = typeof(T);
            var enProArr = enType.GetFields();
            foreach (var en in enProArr) {
                if (en.FieldType.Equals(enType)) {
                    list.Add((T)Enum.Parse(enType, en.Name));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取所有枚举和对应的属性
        /// </summary>
        /// <returns></returns>
        public static List<EnumItem<T>> GetAllEnumItem() {
            List<EnumItem<T>> list = new List<EnumItem<T>>();
            Type enType = typeof(T);
            var enumAttrType = typeof(EnumAttr);
            var enProArr = enType.GetFields();
            foreach (var en in enProArr) {
                if (en.FieldType.Equals(enType)) {
                    T em = (T)Enum.Parse(enType, en.Name);
                    var attrs = en.GetCustomAttributes(enumAttrType, false);
                    if (attrs == null || attrs.Length <= 0) continue;
                    EnumAttr attr = (EnumAttr)attrs[0];
                    list.Add(new EnumItem<T> { Enum = em, Attr = attr });
                }
            }
            return list.OrderBy(e => e.Enum).ToList();
        }

        /// <summary>
        /// 获取枚举的中文文本(Text)和枚举中文值(Value)映射的下拉菜单
        /// </summary>
        /// <returns></returns>
        public static List<ListItem> AllItemToListItem() {
            return GetAllEnumItem().Select(e => new ListItem(e.Text, e.Enum.GetInt(0, false).ToString())).ToList();
        }

    }
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumHelper {

        /// <summary>
        /// [已废弃,请使用EnumHelper&lt;T&gt;.AllItemToListItem()]获取所有枚举和对应的属性
        /// </summary>
        /// <returns></returns>
        public static List<ListItem> AllItemToListItem(this Enum en) {
            List<ListItem> list = new List<ListItem>();

            if (en.IsEnumEmpty()) return list;

            Type enType = en.GetType();
            var enProArr = enType.GetFields();
            foreach (var enAttr in enProArr) {
                if (enAttr.FieldType.Equals(enType)) {
                    Enum enumObj = (Enum)Enum.Parse(enType, enAttr.Name);
                    var enumObjAttr = enumObj.GetCustomAttribute<EnumAttr>();
                    list.Add(new ListItem(enumObjAttr.Text, enumObj.GetInt(0, false).ToString()));
                }
            }
            return list;
        }

        /// <summary> 
        /// 获取枚举项的Attribute 
        /// </summary> 
        /// <typeparam name="T">自定义的Attribute</typeparam> 
        /// <param name="source">枚举</param> 
        /// <returns>返回枚举,否则返回null</returns> 
        public static T GetCustomAttribute<T>(this Enum source) where T : Attribute {
            return (T)GetCustomAttribute(source, source.GetType(), typeof(T));
        }
        /// <summary> 
        /// 获取枚举项的Attribute 
        /// </summary> 
        /// <param name="source"></param> 
        /// <param name="sourceType"></param> 
        /// <param name="attributeType"></param> 
        /// <returns>返回枚举,否则返回null</returns> 
        public static object GetCustomAttribute(this Enum source, Type sourceType, Type attributeType) {
            string sourceName = Enum.GetName(sourceType, source);
            FieldInfo field = sourceType.GetField(sourceName);
            object[] attributes = field.GetCustomAttributes(attributeType, false);
            foreach (object attribute in attributes) {
                if (attribute.GetType().Equals(attributeType))
                    return attribute;
            }
            return null;
        }
        /// <summary> 
        /// 获取枚举项的EnumAttr
        /// </summary> 
        /// <param name="source">枚举</param> 
        /// <returns>返回枚举,否则返回null</returns> 
        public static EnumAttr GetEnumAttr(this Enum source) {
            return source.GetCustomAttribute<EnumAttr>();
        }
        /// <summary> 
        /// 获取枚举项的EnumItem
        /// </summary> 
        /// <param name="source">枚举</param> 
        /// <returns>返回枚举,否则返回null</returns> 
        public static EnumItem<T> GetEnumItem<T>(this Enum source) {
            return new EnumItem<T> { Enum = (T)Convert.ChangeType(source, typeof(T)), Attr = source.GetCustomAttribute<EnumAttr>() };
        }

        /// <summary>
        /// 根据名字解析为枚举,同时支持枚举值转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string enumName) {
            try {
                return (T)Enum.Parse(typeof(T), enumName);
            } catch { }
            return default(T);
        }

        /// <summary>
        /// 是否枚举为空
        /// </summary>
        /// <param name="enu"></param>
        /// <returns></returns>
        public static bool IsEnumEmpty(this object enu) {
            try {
                return !Enum.IsDefined(enu.GetType(), enu);
            } catch { }
            return true;
        }
        /// <summary>
        /// 强制转换为枚举类型,如果要用名字解析,请用ParseEnum
        /// </summary>
        /// <param name="enu"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this object enu) {
            try {
                return (T)enu;
            } catch { }
            return default(T);
        }
    }
    /// <summary>
    /// 枚举的属性
    /// </summary>
    public class EnumAttr : Attribute {
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 说明字符串(比Text长)
        /// </summary>
        public string Desc { get; set; }
    }

    /// <summary>
    /// 枚举项(包括枚举和对应的属性)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumItem<T> {
        /// <summary>
        /// 枚举
        /// </summary>
        public T Enum { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get { return Attr.Text;} }
        /// <summary>
        /// 枚举的EnumAttr属性对象
        /// </summary>
        public EnumAttr Attr { get; set; }
    }
}
