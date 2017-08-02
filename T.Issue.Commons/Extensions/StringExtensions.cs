using System;

namespace T.Issue.Commons.Extensions
{
    public static class StringExtensions
    {
        public static string SafeSubstring(this string s, int length)
        {
            if (null == s)
            {
                return null;
            }

            if (length < s.Length)
            {
                return s.Substring(0, length);
            }

            return s;
        }

        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        public static string Truncate(this string str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
            }

            if (str == null)
            {
                return null;
            }

            int maxLength = Math.Min(str.Length, length);
            return str.Substring(0, maxLength);
        }
    }
}
