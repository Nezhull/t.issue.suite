using System;
using System.Linq;

namespace T.Issue.Commons.Utils
{
    public static class StringUtils
    {
        public static string Join(string separator, params object[] values)
        {
            if (values[0] == null)
            {
                values[0] = string.Empty;
            }

            return string.Join(separator, values);
        }

        public static string JoinNotEmpty(string separator, params object[] values)
        {
            return string.Join(separator, values.Where(v => v != null).Select(v => v.ToString()).Where(v => !Equals(v, string.Empty)).ToList());
        }

        /// <summary>
        /// Converts hexadecimal string to byte array.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        /// <summary>
        /// Converts byte array to hexadecimal string.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ByteArrayToHex(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", string.Empty).ToLower();
        }
    }
}
