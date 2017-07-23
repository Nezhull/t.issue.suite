using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace T.Issue.Commons.Utils.Tests
{
    [TestClass]
    public class StringUtilsTests
    {
        [TestMethod]
        public void TestHexConversion1()
        {
            string str = "This is test!";

            byte[] bytes = Encoding.UTF8.GetBytes(str);

            string hex = StringUtils.ByteArrayToHex(bytes);

            byte[] fromHex = StringUtils.HexToByteArray(hex);

            string strFromHex = Encoding.UTF8.GetString(fromHex);
            
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(str, strFromHex);
        }

        [TestMethod]
        public void TestHexConversion2()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();

            string hex = StringUtils.ByteArrayToHex(bytes);

            byte[] fromHex = StringUtils.HexToByteArray(hex);
            
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(bytes.SequenceEqual(fromHex));
        }
    }
}
