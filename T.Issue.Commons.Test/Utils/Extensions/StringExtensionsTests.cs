using T.Issue.Commons.Extensions;
using Xunit;

namespace T.Issue.Commons.Test.Utils.Extensions
{
    public class StringExtensionsTests
    {
        private const string S12 = "length is 12";

        private const string S12Substring06 = "length";

        [Fact]
        public void SafeSubstring_returns_null_when_string_is_null()
        {
            string s = null;
            string expected = null;

            var actual = s.SafeSubstring(7);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeSubstring_returns_string_when_string_is_shorter_than_length()
        {
            const string expected = S12;

            var actual = S12.SafeSubstring(21);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeSubstring_returns_substring_from_the_beginning()
        {
            const string expected = S12Substring06;

            var actual = S12.SafeSubstring(6);

            Assert.Equal(expected, actual);
        }
    }
}
