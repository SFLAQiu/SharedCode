//=============================================================================== 
// 
// 从 DataGrid    或数据源中导出数据到 Excel 并提示下载的帮助类。 
// Version: 1.22 
// History: 
//            v1.00    使用静态方法的形式实现该类，提供多种重载方式。 
//            v1.01    添加了对 DevExpress.Web.ASPxGrid.ASPxGrid 的直接导出支持。 
//            v1.20    改写为实体类。 减少了重复代码。 
//            v1.21    2005-2-1     
//                    修改了一个构造函数的重载形式中异常检测的代码。延迟到 Export() 方法。 
//            v1.22    2005-2-3     
//                    1. 修正了 Export() 方法中缺少 _titles != null 判断的 bug. 
//                    2. 修正了长的数字被 Excel 自动转换为科学计数法的毛病。 
//                        (修改的办法来自 http://dotnet.aspx.cc) 
// 
//=============================================================================== 
using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using System.Collections;

namespace LG.Utility
{ 

    public class ExcelHelper { 

        #region 字段
 
        string _fileName; 
        DataTable _dataSource;         
        string[] _titles = null; 
        string[] _fields = null; 
        int _maxRecords = 1000; 
 
        #endregion 
 
        #region 属性 
 
        /// <summary> 
        /// 限制输出到 Excel 的最大记录数。超出则抛出异常 
        /// </summary> 
        public int MaxRecords { 
            set { _maxRecords = value; } 
            get { return _maxRecords; } 
        } 
 
        /// <summary> 
        /// 输出到浏览器的 Excel 文件名 
        /// </summary> 
        public string FileName { 
            set { _fileName = value; } 
            get { return _fileName; } 
        } 
 
        #endregion 
 
        #region 构造函数 
 
        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="titles">要输出到 Excel 的列标题的数组</param> 
        /// <param name="fields">要输出到 Excel 的字段名称数组</param> 
        /// <param name="dataSource">数据源</param> 
        public ExcelHelper(string[] titles, string[] fields, DataTable dataSource): this(titles, dataSource)        { 
            if (fields == null || fields.Length == 0) 
                throw new ArgumentNullException("fields"); 
 
            if (titles.Length != fields.Length) 
                throw new ArgumentException("titles.Length != fields.Length", "fields"); 
             
            _fields = fields;             
        } 
 
        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="titles">要输出到 Excel 的列标题的数组</param> 
        /// <param name="dataSource">数据源</param> 
        public ExcelHelper(string[] titles, DataTable dataSource): this(dataSource) { 
            if (titles == null || titles.Length == 0) 
                throw new ArgumentNullException("titles"); 
            //if (titles.Length != dataSource.Columns.Count) 
            //    throw new ArgumentException("titles.Length != dataSource.Columns.Count", "dataSource"); 
 
            _titles = titles;             
        } 
 
        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="dataSource">数据源</param> 
        public ExcelHelper(DataTable dataSource) { 
            if (dataSource == null) 
                throw new ArgumentNullException("dataSource"); 
            // maybe more checks needed here (IEnumerable, IList, IListSource, ) ??? 
            // 很难判断，先简单的使用 DataTable 
 
            _dataSource = dataSource; 
        } 
         
        public ExcelHelper() {} 
 
        #endregion 
         
        #region 公有方法
         
        /// <summary> 
        /// 导出到 Excel 并提示下载 
        /// </summary> 
        /// <param name="dg">DataGrid</param> 
        public void Export(DataGrid dg) { 
            if (dg == null) 
                throw new ArgumentNullException("dg"); 
            if (dg.AllowPaging || dg.PageCount > 1) 
                throw new ArgumentException("paged DataGrid can't be exported.", "dg"); 
 
            // 添加标题样式 
            dg.HeaderStyle.Font.Bold = true; 
            dg.HeaderStyle.BackColor = System.Drawing.Color.LightGray; 
 
            RenderExcel(dg); 
        }
    
//        /// <summary> 
//        /// 导出到 Excel 并提示下载 
//        /// </summary> 
//        /// <param name="xgrid">ASPxGrid</param> 
//        public void Export(DevExpress.Web.ASPxGrid.ASPxGrid xgrid) {  
//            if (xgrid == null) 
//                throw new ArgumentNullException("xgrid"); 
//            if (xgrid.PageCount > 1) 
//                throw new ArgumentException("paged xgird not can't be exported.", "xgrid"); 
// 
//            // 添加标题样式 
//            xgrid.HeaderStyle.Font.Bold = true; 
//            xgrid.HeaderStyle.BackColor = System.Drawing.Color.LightGray; 
// 
//            RenderExcel(xgrid); 
//        } 
 
        /// <summary> 
        /// 导出到 Excel 并提示下载 
        /// </summary> 
        public void Export() { 
            if (_dataSource == null) 
                throw new Exception("数据源尚未初始化"); 
 
            if (_fields == null && _titles != null && _titles.Length != _dataSource.Columns.Count)  
                throw new Exception("_titles.Length != _dataSource.Columns.Count"); 
             
            if (_dataSource.Rows.Count > _maxRecords) 
                throw new Exception("导出数据条数超过限制。请设置 MaxRecords 属性以定义导出的最多记录数。"); 
 
            DataGrid dg = new DataGrid(); 
            dg.DataSource = _dataSource; 
 
            if (_titles == null) { 
                dg.AutoGenerateColumns = true; 
            }  
            else { 
                dg.AutoGenerateColumns = false; 
                int cnt = _titles.Length; 
 
                System.Web.UI.WebControls.BoundColumn col; 
 
                if (_fields == null) { 
                    for (int i=0; i<cnt; i++) { 
                        col = new System.Web.UI.WebControls.BoundColumn(); 
                        col.HeaderText = _titles[i]; 
                        col.DataField = _dataSource.Columns[i].ColumnName; 
                        dg.Columns.Add(col); 
                    } 
                } 
                else { 
                    for (int i=0; i<cnt; i++) { 
                        col = new System.Web.UI.WebControls.BoundColumn(); 
                        col.HeaderText = _titles[i]; 
                        col.DataField = _fields[i]; 
                        dg.Columns.Add(col); 
                    } 
                } 
            } 
 
            // 添加标题样式 
            dg.HeaderStyle.Font.Bold = true; 
            dg.HeaderStyle.BackColor = System.Drawing.Color.LightGray; 
            dg.ItemDataBound += new DataGridItemEventHandler(DataGridItemDataBound); 
 
            dg.DataBind(); 
            RenderExcel(dg); 
        } 
 
        #endregion 
 
        #region 私有方法 
         
        private void RenderExcel(System.Web.UI.Control c)
        { 
            // 确保有一个合法的输出文件名 
            if (_fileName == null || _fileName == string.Empty || !(_fileName.ToLower().EndsWith(".xls"))) 
                _fileName = GetRandomFileName(); 
 
            HttpResponse response = HttpContext.Current.Response; 
             
            response.Charset = "GB2312"; 
            response.ContentEncoding = Encoding.GetEncoding("GB2312"); 
            response.ContentType = "application/ms-excel/msword"; 
            response.AppendHeader("Content-Disposition", "attachment;filename=" +  
                HttpUtility.UrlEncode(_fileName)); 
 
            CultureInfo cult = new CultureInfo("zh-CN", true); 
            StringWriter sw = new StringWriter(cult);             
            HtmlTextWriter writer = new HtmlTextWriter(sw); 
 
            writer.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=GB2312\">"); 
 
            DataGrid dg = c as DataGrid; 
             
            if (dg != null) 
            { 
                dg.RenderControl(writer); 
            } 
            else 
            {
                throw new ArgumentException("only supports DataGrid or ASPxGrid.", "c");     
            } 

            c.Dispose(); 
 
            response.Write(sw.ToString()); 
            response.End(); 
        } 
 
 
        /// <summary> 
        /// 得到一个随意的文件名 
        /// </summary> 
        /// <returns></returns> 
        private string GetRandomFileName() { 
            Random rnd = new Random((int) (DateTime.Now.Ticks)); 
            string s = rnd.Next(Int32.MaxValue).ToString(); 
            return DateTime.Now.ToShortDateString() + "_" + s + ".xls"; 
        } 
 
        private void DataGridItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) { 
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) { 
                e.Item.Attributes.Add("style", "vnd.ms-excel.numberformat:@"); 
                //e.Item.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:￥#,###.00"); 
            } 
        } 
 
        #endregion 
    } 
}