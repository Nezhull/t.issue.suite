using System;
using System.Linq;
using System.Text;
using T.Issue.Commons.Utils;
using Xunit;
using Assert = Xunit.Assert;

namespace T.Issue.Commons.Test.Utils
{
    public class StringUtilsTests
    {
        [Fact]
        public void TestHexConversion1()
        {
            string str = "This is test!";

            byte[] bytes = Encoding.UTF8.GetBytes(str);

            string hex = StringUtils.ByteArrayToHex(bytes);

            byte[] fromHex = StringUtils.HexToByteArray(hex);

            string strFromHex = Encoding.UTF8.GetString(fromHex);
            
            Assert.Equal(str, strFromHex);
        }

        [Fact]
        public void TestHexConversion2()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();

            string hex = StringUtils.ByteArrayToHex(bytes);

            byte[] fromHex = StringUtils.HexToByteArray(hex);
            
            Assert.True(bytes.SequenceEqual(fromHex));
        }
    }
}
