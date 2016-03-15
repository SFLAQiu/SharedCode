using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
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
        /// <summary>
        /// 转半角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e) {
            MessageBox.Show(this.txt_qb.Text.ToDBC());
        }
        /// <summary>
        /// 转全角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e) {
            MessageBox.Show(Test(d => d == this.txt_qb.Text, "2").ToString());
           // MessageBox.Show(this.txt_qb.Text.ToSBC());
        }
        public bool Test(Func<string,bool> fc,string str) {
            return fc.Invoke(str);
        }

        private void button8_Click(object sender, EventArgs e) {
           
            ////jCwZlFi+ImzRYerQHMsaw6mN3PWlx3Sr|1447310524014
            ////混淆加密
            //SuperEncrypt se = new SuperEncrypt("C6G89MB6PI3X5YF6");
            ////明文
            //var plaintext = "token,1447310524014";
            ////密文
            //string keyStr=string.Empty;//秘钥
            //var ciphertext = se.Encrypt(plaintext, out keyStr);
            ////根据密文解析秘钥
            //var secretKey = SuperEncrypt.GetSecretKey(ciphertext);
            ////解密
            //var str = se.Decrypt(ciphertext);
            //this.txt_mw.Text =
            // @"秘钥=" + SuperEncrypt.GetSecretKey(ciphertext)+
            //"加密串=" + ciphertext + "\r\n" +
            //"url编码=" + ciphertext.UrlDecode() + "\r\n" +
            //"解密=" + str;
            string a ="23.365";
            MessageBox.Show(a.To<int>().ToString());
        }

        private void button9_Click(object sender, EventArgs e) {
            this.txt_md5_result.Text = this.txt_MD5.Text.MD5().ToLower();
        }

        private void btn_IP_Click(object sender, EventArgs e) {
            MessageBox.Show(StaticFunctions.GetClientRealIpV2());
        }

        private void button10_Click(object sender, EventArgs e) {
            //var datas = new SortedList<int, string>();
            //datas.Add(1, "1");
            //datas.Add(3, "3");
            //datas.Add(9, "9");
            //datas.Add(2, "2");
            //datas.Add(11, "11");
            //datas.Add(4, "4");
            //StringBuilder str = new StringBuilder();
            //foreach (var item in datas) {
            //    datas.
            //    str.Append(item.Value+"|");
            //}
            //MessageBox.Show(str.ToString());

            //List<int> datas = new List<int> { 
            //    1,2,36,787,3,55,21,456,23,2,4,56,12,5,6 nr6
            //};
            //var rtnDatas = datas.OrderBy(s => Guid.NewGuid()).ToList();
            //MessageBox.Show(rtnDatas[0].ToString()+",guid="+Guid.NewGuid().ToString());
            //var a = 10m*0.36m;
            //var aa = (int)(a * 10);
            //var c = Math.Round(a, 1, MidpointRounding.AwayFromZero);

            //MessageBox.Show(((int)(a * 10) / 10.0).ToString("f1"));
           JavaScriptSerializer serializer = new JavaScriptSerializer();
            var cookie = new System.Net.CookieCollection();
            var result = HttpAccessHelper.GetHttpPostJsonText("http://gw.fanhuan.com/common/PushPassthrough/?uids=11269864", Encoding.UTF8, new { module = "aaa", value = "d1" }.GetJSON(),null,out cookie, timeoutMillisecond: 30000);
            MessageBox.Show(result);
        }





        private void tabPage1_Click(object sender, EventArgs e) {

        }

    }
    public class imgDemo {
        public int ImgHeight { get; set; }
        public int ImgWidth { get; set; }
    }
}
