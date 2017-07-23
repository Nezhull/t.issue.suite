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
        /// Standard format for date and time.
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Formats <see cref="DateTime"/> using <see cref="DateFormat"/> format, <code>null</code> safe.
        /// </summary>
        /// <param name="date">Date to be formatted.</param>
        /// <returns>Date string.</returns>
        public static string FormatDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }
            return FormatDate(date.Value);
        }

        /// <summary>
        /// Formats <see cref="DateTime"/> using <see cref="DateTimeFormat"/> format, <code>null</code> safe.
        /// </summary>
        /// <param name="dateTime">Date and time to be formatted.</param>
        /// <returns>Date and time string.</returns>
        public static string FormatDateTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return string.Empty;
            }
            return FormatDateTime(dateTime.Value);
        }

        /// <summary>
        /// Formats <see cref="DateTime"/> using <see cref="DateFormat"/> format.
        /// </summary>
        /// <param name="date">Date to be formatted.</param>
        /// <returns>Date string.</returns>
        public static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }

        /// <summary>
        /// Formats <see cref="DateTime"/> using <see cref="DateTimeFormat"/> format.
        /// </summary>
        /// <param name="dateTime">Date and time to be formatted.</param>
        /// <returns>Date and time string.</returns>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }

        /// <summary>
        /// Formats <see cref="TimeSpan"/> using <see cref="TimeSpanFormat"/> format, <code>null</code> safe.
        /// </summary>
        /// <param name="timeSpan">Timespan to be formatted.</param>
        /// <returns>Timespan string.</returns>
        public static string FormatTimeSpan(TimeSpan? timeSpan)
        {
            if (!timeSpan.HasValue)
            {
                return string.Empty;
            }
            return FormatTimeSpan(timeSpan.Value);
        }

        /// <summary>
        /// Formats <see cref="TimeSpan"/> using <see cref="TimeSpanFormat"/> format.
        /// </summary>
        /// <param name="timeSpan">Timespan to be formatted.</param>
        /// <returns>Timespan string.</returns>
        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return timeSpan.ToString(TimeSpanFormat);
        }

        /// <summary>
        /// Parses <see cref="DateTime"/> using <see cref="DateTimeFormat"/> date and time format.
        /// </summary>
        /// <param name="dateTimeStr">Date and time to be parsed.</param>
        /// <returns>Date and time object.</returns>
        public static DateTime? ParseDateTime(string dateTimeStr)
        {
            return ParseDateTime(dateTimeStr, DateTimeFormat);
        }

        /// <summary>
        /// Parses <see cref="DateTime"/> using custom date and time format.
        /// </summary>
        /// <param name="dateTimeStr">Date and time to be parsed.</param>
        /// <param name="format">Custom date and time format.</param>
        /// <returns>Date and time object.</returns>
        public static DateTime? ParseDateTime(string dateTimeStr, string format)
        {
            Assert.HasText(format);

            DateTime dateTime;

            if (DateTime.TryParseExact(dateTimeStr, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        /// <summary>
        /// Formats date range, <code>null</code> safe.
        /// </summary>
        /// <param name="from">Date from.</param>
        /// <param name="to">Date to.</param>
        /// <param name="pattern">Formatting pattern.</param>
        /// <returns></returns>
        public static string FormatDateRange(DateTime? from, DateTime? to, string pattern = "{0} - {1}")
        {
            if (!from.HasValue && !to.HasValue)
            {
                return string.Empty;
            }

            return string.Format(pattern, FormatDate(from), FormatDate(to));
        }

        /// <summary>
        /// Formats date range.
        /// </summary>
        /// <param name="from">Date from.</param>
        /// <param name="to">Date to.</param>
        /// <param name="pattern">Formatting pattern.</param>
        /// <returns></returns>
        public static string FormatDateRange(DateTime from, DateTime to, string pattern = "{0} - {1}")
        {
            return string.Format(pattern, FormatDate(from), FormatDate(to));
        }
    }
}
