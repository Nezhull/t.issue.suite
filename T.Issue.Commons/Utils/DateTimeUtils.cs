using System;
using System.Globalization;

namespace T.Issue.Commons.Utils
{
    /// <summary>
    /// Utility class for <see cref="DateTime"/> common utility methods.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Standard format for timespan.
        /// </summary>
        public const string TimeSpanFormat = "hh:mm";

        /// <summary>
        /// Standard format for date.
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Standard format for date and time with seconds.
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Formats <see cref="DateTime"/> using <see cref="DateFormat"/> format, <code>null</code> safe.
        /// </summary>
        /// <param name="date">Date to be formatted.</param>
        /// <param name="format"></param>
        /// <param name="culture"></param>
        /// <returns>Date string.</returns>
        public static string FormatDateTime(DateTime? date, string format = DateTimeFormat, CultureInfo culture = null)
        {
            return !date.HasValue ? string.Empty : FormatDateTime(date.Value, format, culture);
        }

        /// <summary>
        /// Formats <see cref="DateTime"/> using provided format.
        /// </summary>
        /// <param name="date">Date to be formatted.</param>
        /// <param name="format"></param>
        /// <param name="culture"></param>
        /// <returns>Date string.</returns>
        public static string FormatDateTime(DateTime date, string format = DateTimeFormat, CultureInfo culture = null)
        {
            Assert.HasText(format);

            return date.ToString(format, culture ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Formats <see cref="TimeSpan"/> using <see cref="TimeSpanFormat"/> format, <code>null</code> safe.
        /// </summary>
        /// <param name="timeSpan">Timespan to be formatted.</param>
        /// <param name="format"></param>
        /// <param name="culture"></param>
        /// <returns>Timespan string.</returns>
        public static string FormatTimeSpan(TimeSpan? timeSpan, string format = TimeSpanFormat, CultureInfo culture = null)
        {
            return !timeSpan.HasValue ? string.Empty : FormatTimeSpan(timeSpan.Value, format, culture);
        }

        /// <summary>
        /// Formats <see cref="TimeSpan"/> using <see cref="TimeSpanFormat"/> format.
        /// </summary>
        /// <param name="timeSpan">Timespan to be formatted.</param>
        /// <param name="format"></param>
        /// <param name="culture"></param>
        /// <returns>Timespan string.</returns>
        public static string FormatTimeSpan(TimeSpan timeSpan, string format = TimeSpanFormat, CultureInfo culture = null)
        {
            Assert.HasText(format);

            return timeSpan.ToString(TimeSpanFormat, culture ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses <see cref="DateTime"/> using custom date and time format.
        /// </summary>
        /// <param name="dateTimeStr">Date and time to be parsed.</param>
        /// <param name="format">Custom date and time format.</param>
        /// <param name="culture"></param>
        /// <param name="style"></param>
        /// <returns>Date and time object.</returns>
        public static DateTime? ParseDateTime(string dateTimeStr, string format = DateTimeFormat, CultureInfo culture = null, DateTimeStyles style = DateTimeStyles.None)
        {
            Assert.HasText(format);

            DateTime dateTime;

            if (DateTime.TryParseExact(dateTimeStr, format, culture ?? CultureInfo.CurrentCulture, style, out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        /// <summary>
        /// Formats date range, <c>null</c> safe.
        /// </summary>
        /// <param name="from">Date from.</param>
        /// <param name="to">Date to.</param>
        /// <param name="datePattern">Custom date and time format. Defaults to <see cref="DateFormat"/>.</param>
        /// <param name="rangeSeparator">Custom range separator. Defaults to <c>" - "</c>.</param>
        /// <param name="culture">Culture to use. If <c>null</c> uses <see cref="CultureInfo.CurrentCulture"/>. Defaults to <c>null</c>.</param>
        /// <returns>Formatted date range or empty string if both dates are <c>null</c>.</returns>
        public static string FormatDateRange(DateTime? from, DateTime? to, string datePattern = DateFormat, string rangeSeparator = " - ", CultureInfo culture = null)
        {
            if (!from.HasValue && !to.HasValue)
            {
                return string.Empty;
            }

            return StringUtils.JoinNotEmpty(rangeSeparator, FormatDateTime(from, datePattern, culture), FormatDateTime(to, datePattern, culture));
        }
    }
}
