using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LG.Utility;
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
            str = StaticFunctions.SetPosStatus(str, 5, true);
            str = StaticFunctions.SetPosStatus(str, 4, true);
            str = StaticFunctions.SetPosStatus(str, 3, false);
            str = StaticFunctions.SetPosStatus(str, 7, true);
            str = StaticFunctions.SetPosStatus(str, 7, false);
            var status = StaticFunctions.GetPosStatus(str, 7);
            status = StaticFunctions.GetPosStatus(str, 5);
            status = StaticFunctions.GetPosStatus(str, 0);
            status = StaticFunctions.GetPosStatus(str, 7);

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

    }
    public class Test {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
    }
}
