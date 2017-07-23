using System.IO;
using System.Security.Cryptography;

namespace T.Issue.Commons.Utils
{
    public static class HashUtils
    {
        /// <summary>
        /// Gets a hash of file using SHA1.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetSHA1Hash(string filePath)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return GetHash(filePath, sha1);
            }
        }

        /// <summary>
        /// Gets a hash of stream using SHA1.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] GetSHA1Hash(Stream stream)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return GetHash(stream, sha1);
            }
        }
        
        /// <summary>
        /// Gets a hash of byte array using SHA1.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] GetSHA1Hash(byte[] buffer)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return GetHash(buffer, sha1);
            }
        }

        /// <summary>
        /// Gets a hash of file using MD5.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetMD5Hash(string filePath)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return GetHash(filePath, md5);
            }
        }

        /// <summary>
        /// Gets a hash of stream using MD5.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] GetMD5Hash(Stream stream)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return GetHash(stream, md5);
            }
        }
        
        /// <summary>
        /// Gets a hash of byte array using MD5.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] GetMD5Hash(byte[] buffer)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return GetHash(buffer, md5);
            }
        }

        private static byte[] GetHash(string filePath, HashAlgorithm hasher)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return GetHash(fs, hasher);
            }
        }

        private static byte[] GetHash(Stream s, HashAlgorithm hasher)
        {
            return hasher.ComputeHash(s);
        }

        private static byte[] GetHash(byte[] content, HashAlgorithm hasher)
        {
            return hasher.ComputeHash(content);
        }
    }
}
