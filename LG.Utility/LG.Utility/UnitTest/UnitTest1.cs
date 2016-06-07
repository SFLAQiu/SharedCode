using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LG.Utility;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace UnitTest {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void JSONSerializeTest() {
            Test obj = null;
            var str = (obj).JSONSerializeV3();
            Assert.AreNotEqual("", str, false, "序列化内容不能为空！！");
            var data = str.JSONDeserializeV3<Test>();
            Assert.AreNotEqual(null, data, "反序列化对象不能为空！！");
        }


        [TestMethod]
        public void PosStatusTest() {
            var str = StaticFunctions.InitPostStatus();
            str = StaticFunctions.SetPosStatus(str, 1, true);
            str = StaticFunctions.SetPosStatus(str, 2, true);
            str = StaticFunctions.SetPosStatus(str, 3, false);
            str = StaticFunctions.SetPosStatus(str, 4, true);
            //str = StaticFunctions.SetPosStatus(str, 9, false);
            str = StaticFunctions.SetPosStatus(str, 6, false);
            str = StaticFunctions.SetPosStatus(str, 8, true);
            str = StaticFunctions.SetPosStatus(str, 1, false);
            var status = StaticFunctions.GetPosStatus(str, 1);
            status = StaticFunctions.GetPosStatus(str, 1);
            //status = StaticFunctions.GetPosStatus(str, 9);
            status = StaticFunctions.GetPosStatus(str, 8);

        }
        [TestMethod]
        public void DoFnAgainTest() {
            var i = 0;
            StaticFunctions.DoFnAgain(delegate () {
                i++;
                if (i == 3) return true;
                return false;
            });

        }

        /// <summary>
        /// ascll加密
        /// </summary>
       [TestMethod]
        public void AscllTest() {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("p2", "1");
            datas.Add("p3", "1");
            datas.Add("p1", "1");
            datas.Add("p5", "1");
        }

        /// <summary>
        /// 计算参数签名
        /// </summary>
        /// <param name="params">请求参数集，所有参数必须已转换为字符串类型</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        public static string getSignature(IDictionary<string, string> parameters, string secret) {
            // 先将参数以其参数名的字典序升序进行排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> iterator = sortedParams.GetEnumerator();

            // 遍历排序后的字典，将所有参数按"key=value"格式拼接在一起
            StringBuilder basestring = new StringBuilder();
            while (iterator.MoveNext()) {
                string key = iterator.Current.Key;
                string value = iterator.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)) {
                    basestring.Append(key).Append("=").Append(value);
                }
            }
            basestring.Append(secret);

            // 使用MD5对待签名串求签
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(basestring.ToString()));

            // 将MD5输出的二进制结果转换为小写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                string hex = bytes[i].ToString("x");
                if (hex.Length == 1) {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString();
        }

        [TestMethod]
        public void GetIenumberableTest() {
            List<Test> datas = new List<Test>();
            for (int i = 0; i < 50; i++) {
                datas.Add(new Test {
                    Id = i,
                    Name = "n" + i
                });
            }
            var dataCount = 0;
            var getDatas=LinqHelper.GetIenumberable<Test>(datas,new Func<Test,bool>(delegate(Test item) {
                if (item.Id == 2) return false;
                return true;
            }), new Func<IEnumerable<Test>, IEnumerable<Test>>(delegate(IEnumerable<Test> items) {
                return items.OrderBy(d => d.Id);
            }) , 10, 15, out dataCount, fpageSize: 5);

        }

    }


  
    
    public class Test {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
    }
}
