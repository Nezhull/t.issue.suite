using System;
using System.Globalization;
using T.Issue.Commons.Utils;
using Xunit;
using Assert = Xunit.Assert;

namespace T.Issue.Commons.Test.Utils
{
    public class StringUtilsTests
    {
        [Fact]
        public void TestFormatDateRange()
        {
            const string expected = "2001-02-21 - 2001-02-23";
            DateTime dateFrom = new DateTime(2001, 2, 21, 0, 0, 0);
            DateTime dateTo = new DateTime(2001, 2, 23, 0, 0, 0);

            string formatted = DateTimeUtils.FormatDateRange(dateFrom, dateTo, culture:CultureInfo.InvariantCulture);

            Assert.Equal(expected, formatted);
        }

        [Fact]
        public void TestFormatDateRangeOnlyFrom()
        {
            const string expected = "2001-02-21";
            DateTime dateFrom = new DateTime(2001, 2, 21, 0, 0, 0);

            string formatted = DateTimeUtils.FormatDateRange(dateFrom, null, culture:CultureInfo.InvariantCulture);

            Assert.Equal(expected, formatted);
        }

        [Fact]
        public void TestFormatDateRangeOnlyTo()
        {
            const string expected = "2001-02-23";
            DateTime dateTo = new DateTime(2001, 2, 23, 0, 0, 0);

            string formatted = DateTimeUtils.FormatDateRange(null, dateTo, culture:CultureInfo.InvariantCulture);

            Assert.Equal(expected, formatted);
        }
    }
}
