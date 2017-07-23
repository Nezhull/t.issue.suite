using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace T.Issue.Commons.Utils.Tests
{
    [TestClass]
    public class EncryptionUtilsTests
    {
        private const string Key = "E33FF051A90FC168DF2853407ADB0116";
        private const string Vector = "A25C8CBFB3385259F264E44D8D40C8B4";

        [TestMethod]
        public void TestEncryption1()
        {
            byte[] toEncrypt = Guid.NewGuid().ToByteArray();

            byte[] encrypted = EncryptionUtils.EncryptAES(toEncrypt, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            byte[] decrypted = EncryptionUtils.DecryptAES(encrypted, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(toEncrypt.SequenceEqual(decrypted));
        }

        [TestMethod]
        public void TestEncryption2()
        {
            string toEncryptStr = "This is test!";

            byte[] toEncrypt = Encoding.UTF8.GetBytes(toEncryptStr);

            byte[] encrypted = EncryptionUtils.EncryptAES(toEncrypt, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            byte[] decrypted = EncryptionUtils.DecryptAES(encrypted, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            string decryptedStr = Encoding.UTF8.GetString(decrypted);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(decryptedStr, toEncryptStr);
        }

        [TestMethod]
        public void TestEncryptionWithEntropy()
        {
            string toEncryptStr = "This is test!";

            byte[] toEncryptOrig = Encoding.UTF8.GetBytes(toEncryptStr);
            byte[] toEncrypt = toEncryptOrig.Concat(EncryptionUtils.GenerateEntropy(64)).ToArray();

            byte[] encrypted = EncryptionUtils.EncryptAES(toEncrypt, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            byte[] decrypted = EncryptionUtils.DecryptAES(encrypted, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            string decryptedStr = Encoding.UTF8.GetString(decrypted.Take(toEncryptOrig.Length).ToArray());

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(decryptedStr, toEncryptStr);
        }
    }
}
