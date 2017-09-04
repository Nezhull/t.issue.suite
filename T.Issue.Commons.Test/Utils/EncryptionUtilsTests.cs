using System;
using System.Linq;
using System.Text;
using T.Issue.Commons.Utils;
using Xunit;
using Assert = Xunit.Assert;

namespace T.Issue.Commons.Test.Utils
{
    public class EncryptionUtilsTests
    {
        private const string Key = "E33FF051A90FC168DF2853407ADB0116";
        private const string Vector = "A25C8CBFB3385259F264E44D8D40C8B4";

        [Fact]
        public void TestEncryption1()
        {
            byte[] toEncrypt = Guid.NewGuid().ToByteArray();

            byte[] encrypted = EncryptionUtils.EncryptAES(toEncrypt, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            byte[] decrypted = EncryptionUtils.DecryptAES(encrypted, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            Assert.True(toEncrypt.SequenceEqual(decrypted));
        }

        [Fact]
        public void TestEncryption2()
        {
            string toEncryptStr = "This is test!";

            byte[] toEncrypt = Encoding.UTF8.GetBytes(toEncryptStr);

            byte[] encrypted = EncryptionUtils.EncryptAES(toEncrypt, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            byte[] decrypted = EncryptionUtils.DecryptAES(encrypted, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            string decryptedStr = Encoding.UTF8.GetString(decrypted);

            Assert.Equal(decryptedStr, toEncryptStr);
        }

        [Fact]
        public void TestEncryptionWithEntropy()
        {
            string toEncryptStr = "This is test!";

            byte[] toEncryptOrig = Encoding.UTF8.GetBytes(toEncryptStr);
            byte[] toEncrypt = toEncryptOrig.Concat(EncryptionUtils.GenerateEntropy(64)).ToArray();

            byte[] encrypted = EncryptionUtils.EncryptAES(toEncrypt, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            byte[] decrypted = EncryptionUtils.DecryptAES(encrypted, StringUtils.HexToByteArray(Key), StringUtils.HexToByteArray(Vector));

            string decryptedStr = Encoding.UTF8.GetString(decrypted.Take(toEncryptOrig.Length).ToArray());

            Assert.Equal(decryptedStr, toEncryptStr);
        }
    }
}
