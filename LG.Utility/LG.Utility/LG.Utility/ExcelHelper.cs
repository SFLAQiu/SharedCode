//=============================================================================== 
// 
// �� DataGrid    ������Դ�е������ݵ� Excel ����ʾ���صİ����ࡣ 
// Version: 1.22 
// History: 
//            v1.00    ʹ�þ�̬��������ʽʵ�ָ��࣬�ṩ�������ط�ʽ�� 
//            v1.01    ����˶� DevExpress.Web.ASPxGrid.ASPxGrid ��ֱ�ӵ���֧�֡� 
//            v1.20    ��дΪʵ���ࡣ �������ظ����롣 
//            v1.21    2005-2-1     
//                    �޸���һ�����캯����������ʽ���쳣���Ĵ��롣�ӳٵ� Export() ������ 
//            v1.22    2005-2-3     
//                    1. ������ Export() ������ȱ�� _titles != null �жϵ� bug. 
//                    2. �����˳������ֱ� Excel �Զ�ת��Ϊ��ѧ��������ë���� 
//                        (�޸ĵİ취���� http://dotnet.aspx.cc) 
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

        #region �ֶ�
 
        string _fileName; 
        DataTable _dataSource;         
        string[] _titles = null; 
        string[] _fields = null; 
        int _maxRecords = 1000; 
 
        #endregion 
 
        #region ���� 
 
        /// <summary> 
        /// ��������� Excel ������¼�����������׳��쳣 
        /// </summary> 
        public int MaxRecords { 
            set { _maxRecords = value; } 
            get { return _maxRecords; } 
        } 
 
        /// <summary> 
        /// ������������ Excel �ļ��� 
        /// </summary> 
        public string FileName { 
            set { _fileName = value; } 
            get { return _fileName; } 
        } 
 
        #endregion 
 
        #region ���캯�� 
 
        /// <summary> 
        /// ���캯�� 
        /// </summary> 
        /// <param name="titles">Ҫ����� Excel ���б��������</param> 
        /// <param name="fields">Ҫ����� Excel ���ֶ���������</param> 
        /// <param name="dataSource">����Դ</param> 
        public ExcelHelper(string[] titles, string[] fields, DataTable dataSource): this(titles, dataSource)        { 
            if (fields == null || fields.Length == 0) 
                throw new ArgumentNullException("fields"); 
 
            if (titles.Length != fields.Length) 
                throw new ArgumentException("titles.Length != fields.Length", "fields"); 
             
            _fields = fields;             
        } 
 
        /// <summary> 
        /// ���캯�� 
        /// </summary> 
        /// <param name="titles">Ҫ����� Excel ���б��������</param> 
        /// <param name="dataSource">����Դ</param> 
        public ExcelHelper(string[] titles, DataTable dataSource): this(dataSource) { 
            if (titles == null || titles.Length == 0) 
                throw new ArgumentNullException("titles"); 
            //if (titles.Length != dataSource.Columns.Count) 
            //    throw new ArgumentException("titles.Length != dataSource.Columns.Count", "dataSource"); 
 
            _titles = titles;             
        } 
 
        /// <summary> 
        /// ���캯�� 
        /// </summary> 
        /// <param name="dataSource">����Դ</param> 
        public ExcelHelper(DataTable dataSource) { 
            if (dataSource == null) 
                throw new ArgumentNullException("dataSource"); 
            // maybe more checks needed here (IEnumerable, IList, IListSource, ) ??? 
            // �����жϣ��ȼ򵥵�ʹ�� DataTable 
 
            _dataSource = dataSource; 
        } 
         
        public ExcelHelper() {} 
 
        #endregion 
         
        #region ���з���
         
        /// <summary> 
        /// ������ Excel ����ʾ���� 
        /// </summary> 
        /// <param name="dg">DataGrid</param> 
        public void Export(DataGrid dg) { 
            if (dg == null) 
                throw new ArgumentNullException("dg"); 
            if (dg.AllowPaging || dg.PageCount > 1) 
                throw new ArgumentException("paged DataGrid can't be exported.", "dg"); 
 
            // ��ӱ�����ʽ 
            dg.HeaderStyle.Font.Bold = true; 
            dg.HeaderStyle.BackColor = System.Drawing.Color.LightGray; 
 
            RenderExcel(dg); 
        }
    
//        /// <summary> 
//        /// ������ Excel ����ʾ���� 
//        /// </summary> 
//        /// <param name="xgrid">ASPxGrid</param> 
//        public void Export(DevExpress.Web.ASPxGrid.ASPxGrid xgrid) {  
//            if (xgrid == null) 
//                throw new ArgumentNullException("xgrid"); 
//            if (xgrid.PageCount > 1) 
//                throw new ArgumentException("paged xgird not can't be exported.", "xgrid"); 
// 
//            // ��ӱ�����ʽ 
//            xgrid.HeaderStyle.Font.Bold = true; 
//            xgrid.HeaderStyle.BackColor = System.Drawing.Color.LightGray; 
// 
//            RenderExcel(xgrid); 
//        } 
 
        /// <summary> 
        /// ������ Excel ����ʾ���� 
        /// </summary> 
        public void Export() { 
            if (_dataSource == null) 
                throw new Exception("����Դ��δ��ʼ��"); 
 
            if (_fields == null && _titles != null && _titles.Length != _dataSource.Columns.Count)  
                throw new Exception("_titles.Length != _dataSource.Columns.Count"); 
             
            if (_dataSource.Rows.Count > _maxRecords) 
                throw new Exception("�������������������ơ������� MaxRecords �����Զ��嵼��������¼����"); 
 
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
 
            // ��ӱ�����ʽ 
            dg.HeaderStyle.Font.Bold = true; 
            dg.HeaderStyle.BackColor = System.Drawing.Color.LightGray; 
            dg.ItemDataBound += new DataGridItemEventHandler(DataGridItemDataBound); 
 
            dg.DataBind(); 
            RenderExcel(dg); 
        } 
 
        #endregion 
 
        #region ˽�з��� 
         
        private void RenderExcel(System.Web.UI.Control c)
        { 
            // ȷ����һ���Ϸ�������ļ��� 
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
        /// �õ�һ��������ļ��� 
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
                //e.Item.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:��#,###.00"); 
            } 
        } 
 
        #endregion 
    } 
}