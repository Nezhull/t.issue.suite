using System;
using T.Issue.Commons.Utils;

namespace T.Issue.Commons.Extensions
{
    public static class StringExtensions
    {
        public static string SafeSubstring(this string s, int startIndex, int length)
        {
            if (null == s)
            {
                return null;
            }

            return length < s.Length ? s.Substring(startIndex, length) : s;
        }

        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        /// <exception cref="AssertionException">If <c>length &lt;= 0</c>.</exception>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        public static string Truncate(this string str, int length)
        {
            Assert.IsTrue(length > 0, "Length must be > 0");

            return str?.Substring(0, Math.Min(str.Length, length));
        }
    }
}
