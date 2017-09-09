using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace T.Issue.Commons.Utils
{
    /// <summary>
    ///
    /// </summary>
    public static class HashUtils
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="fileShare"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static byte[] GetHash<T>(string filePath, FileMode fileMode = FileMode.Open,
            FileAccess fileAccess = FileAccess.Read, FileShare fileShare = FileShare.ReadWrite) where T : HashAlgorithm
        {
            using (HashAlgorithm hashAlgorithm = CreateHashAlgorithm<T>())
            {
                using (var fs = new FileStream(filePath, fileMode, fileAccess, fileShare))
                {
                    return GetHash(fs, hashAlgorithm);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static byte[] GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            using (HashAlgorithm hashAlgorithm = CreateHashAlgorithm<T>())
            {
                return GetHash(stream, hashAlgorithm);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static byte[] GetHash<T>(byte[] content) where T : HashAlgorithm
        {
            using (HashAlgorithm hashAlgorithm = CreateHashAlgorithm<T>())
            {
                return GetHash(content, hashAlgorithm);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="hasher"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="fileShare"></param>
        /// <returns></returns>
        public static byte[] GetHash(string filePath, HashAlgorithm hasher, FileMode fileMode = FileMode.Open,
            FileAccess fileAccess = FileAccess.Read, FileShare fileShare = FileShare.ReadWrite)
        {
            using (var fs = new FileStream(filePath, fileMode, fileAccess, fileShare))
            {
                return GetHash(fs, hasher);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <param name="hasher"></param>
        /// <returns></returns>
        public static byte[] GetHash(Stream s, HashAlgorithm hasher)
        {
            return hasher.ComputeHash(s);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="hasher"></param>
        /// <returns></returns>
        public static byte[] GetHash(byte[] content, HashAlgorithm hasher)
        {
            return hasher.ComputeHash(content);
        }

        private static HashAlgorithm CreateHashAlgorithm<T>()
        {
            MethodInfo methodInfo = ReflectionUtils.GetMethod<T>("Create", new Type[0]);
            Assert.IsTrue(methodInfo?.IsStatic, $"Can not resolve hash algorithm for {typeof(T)}");

            HashAlgorithm hashAlgorithm = methodInfo.Invoke(null, new object[0]) as HashAlgorithm;
            Assert.NotNull(hashAlgorithm, $"Can not create hash algorithm for {typeof(T)}");

            return hashAlgorithm;
        }
    }
}
