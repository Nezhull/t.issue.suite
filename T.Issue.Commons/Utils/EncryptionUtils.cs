using System.IO;
using System.Security.Cryptography;

namespace T.Issue.Commons.Utils
{
    public static class EncryptionUtils
    {
        public static byte[] EncryptAES(byte[] buffer, byte[] key, byte[] vector)
        {
            Assert.IsNotEmpty(buffer);
            Assert.IsNotEmpty(key);
            Assert.IsNotEmpty(vector);

            byte[] encrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = vector;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (MemoryStream ms = new MemoryStream(buffer))
                        {
                            ms.CopyTo(csEncrypt);
                            csEncrypt.FlushFinalBlock();
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
            }

            return encrypted;
        }

        public static byte[] DecryptAES(byte[] buffer, byte[] key, byte[] vector)
        {
            byte[] decrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = vector;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(buffer))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            csDecrypt.CopyTo(ms);
                            csDecrypt.Flush();
                            decrypted = ms.ToArray();
                        }
                    }
                }
            }

            return decrypted;
        }

        public static byte[] GenerateVectorAES()
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                return aes.IV;
            }
        }

        public static byte[] GenerateEntropy(long length)
        {
            byte[] entropy = new byte[length];

            using (RNGCryptoServiceProvider secureRandom = new RNGCryptoServiceProvider())
            {
                secureRandom.GetNonZeroBytes(entropy);
            }

            return entropy;
        }
    }
}
