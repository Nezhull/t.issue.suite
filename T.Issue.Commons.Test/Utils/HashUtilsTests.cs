using System.Security.Cryptography;
using System.Text;
using T.Issue.Commons.Utils;
using Xunit;
using Assert = Xunit.Assert;

namespace T.Issue.Commons.Test.Utils
{
    public class HashUtilsTests
    {
        [Fact]
        public void TestSHA1Hash()
        {
            string str = "This is test!";
            string expectedHash = "0111b35f1efe624397bbbfa1890f36d2ce690303";

            byte[] hash = HashUtils.GetHash<SHA1>(Encoding.UTF8.GetBytes(str));
            string hashStr = StringUtils.ByteArrayToHex(hash);

            Assert.Equal(expectedHash, hashStr);
        }

        [Fact]
        public void TestSHA256Hash()
        {
            string str = "This is test!";
            string expectedHash = "1adc4dbabf8de40d3c48060fd2366e3cfb7c04bb8aa3fceefa687a1b720b16ae";

            byte[] hash = HashUtils.GetHash<SHA256>(Encoding.UTF8.GetBytes(str));
            string hashStr = StringUtils.ByteArrayToHex(hash);

            Assert.Equal(expectedHash, hashStr);
        }

        [Fact]
        public void TestSHA384Hash()
        {
            string str = "This is test!";
            string expectedHash = "a645556adb2b27fd12b9b98984c6b1fcc6d818fec92c50f0496f4d9cd31ec7869598afa725896c768403e9c652e496c0";

            byte[] hash = HashUtils.GetHash<SHA384>(Encoding.UTF8.GetBytes(str));
            string hashStr = StringUtils.ByteArrayToHex(hash);

            Assert.Equal(expectedHash, hashStr);
        }

        [Fact]
        public void TestSHA512Hash()
        {
            string str = "This is test!";
            string expectedHash = "7d2fdab986200c2625e794b2d6cf47f6d8e4dd084838c1b757ace3102770c99af9300b052418319fb4eca29b7d38ad4c50fdaa87102f43f6a940de53f2767705";

            byte[] hash = HashUtils.GetHash<SHA512>(Encoding.UTF8.GetBytes(str));
            string hashStr = StringUtils.ByteArrayToHex(hash);

            Assert.Equal(expectedHash, hashStr);
        }

        [Fact]
        public void TestMD5Hash()
        {
            string str = "This is test!";
            string expectedHash = "0f1c68b606d650c4d4926d45a5f7182a";

            byte[] hash = HashUtils.GetHash<MD5>(Encoding.UTF8.GetBytes(str));
            string hashStr = StringUtils.ByteArrayToHex(hash);

            Assert.Equal(expectedHash, hashStr);
        }

#if NET45
        [Fact]
        public void TestRIPEMD160Hash()
        {
            string str = "This is test!";
            string expectedHash = "b125063ec2afe25ec11ef2a9acd0b957e374af37";

            byte[] hash = HashUtils.GetHash<RIPEMD160>(Encoding.UTF8.GetBytes(str));
            string hashStr = StringUtils.ByteArrayToHex(hash);

            Assert.Equal(expectedHash, hashStr);
        }
#endif
    }
}
