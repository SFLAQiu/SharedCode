using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LG.Utility;
using Test.Model;
namespace Test {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        /// <summary>
        /// 测试字符串获取url的域名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
             this.lb_URLHost.Text=this.txt_URL.Text.GetUrlHost(needFirstHost: false);
        }
        /// <summary>
        ///测试 发请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            string url = this.txt_RequestURL.Text;
            string str = HttpAccessHelper.GetHttpGetResponseText(url, Encoding.UTF8, 100000);
            string jsonStr = str.RegexReplace(@"^callback\(.+?\""Msg\""\:\""(.*?)\"",.+\);$", "$1").Replace("\\", string.Empty);//Regex.Replace(str, @"^callback\(.+?\""Msg\""\:\""(.*?)\"",.+\);$", "$1").Replace("\\",string.Empty);
            var data = jsonStr.JSONDeserialize<List<ProductClickLog>>();
            if (data == null || data.Count <= 0) return;
            foreach (var item in data) {
                this.lb_URLResponse.Text = this.lb_URLResponse.Text + item.Count.ToString()+"|";
            }
        }
        /// <summary>
        /// Linq多字段分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e) {
            List<JsonResult> lis = new List<JsonResult>();
            lis.Add(new JsonResult { Code = 2, Msg = "1", Callback = "8888888888" });
            lis.Add(new JsonResult { Code = 1, Msg = "1", Callback = "33333333" });
            lis.Add(new JsonResult { Code = 1, Msg = "2", Callback = "44444444444" });
            lis.Add(new JsonResult { Code = 1, Msg = "1", Callback = "22222222" });
            lis.Add(new JsonResult { Code = 1, Msg = "1", Callback = "1111111111" });
            lis.Add(new JsonResult { Code = 1, Msg = "2", Callback = "5555555555555" });
            lis.Add(new JsonResult { Code = 1, Msg = "1", Callback = "6666666666666" });
            lis.Add(new JsonResult { Code = 2, Msg = "1", Callback = "77777777777" });
            var datas = from item in lis
                        group item by new { item.Code, item.Msg } into g
                        select new { Code = g.Key.Code, Msg = g.Key.Msg };
            foreach (var item in datas) {
                this.listBox1.Items.Add("Code="+item.Code+",Msg="+item.Msg);
            }
        }
        /// <summary>
        /// ToListSplit测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e) {
            string str = "1,2,3,4,5,6";
            var lis = str.ToSplitList<int>(',');
            var a = lis;
        }
        /// <summary>
        /// 拼音转中文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e) {
            MessageBox.Show("拼音缩写=" + this.txt_Str.Text.GetPinyinCode());//"全拼=" + this.txt_Str.Text.GetPinyin() + ",=" + this.txt_Str.Text.GetShortPinyin() + 
        }

    }
}
