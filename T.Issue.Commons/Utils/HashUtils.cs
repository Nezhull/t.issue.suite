using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace T.Issue.Commons.Utils
{
    public static class HashUtils
    {
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

        public static byte[] GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            using (HashAlgorithm hashAlgorithm = CreateHashAlgorithm<T>())
            {
                return GetHash(stream, hashAlgorithm);
            }
        }

        public static byte[] GetHash<T>(byte[] content) where T : HashAlgorithm
        {
            using (HashAlgorithm hashAlgorithm = CreateHashAlgorithm<T>())
            {
                return GetHash(content, hashAlgorithm);
            }
        }

        public static byte[] GetHash(string filePath, HashAlgorithm hasher, FileMode fileMode = FileMode.Open,
            FileAccess fileAccess = FileAccess.Read, FileShare fileShare = FileShare.ReadWrite)
        {
            using (var fs = new FileStream(filePath, fileMode, fileAccess, fileShare))
            {
                return GetHash(fs, hasher);
            }
        }

        public static byte[] GetHash(Stream s, HashAlgorithm hasher)
        {
            return hasher.ComputeHash(s);
        }

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
