using System;
using System.Collections;
using System.Text;

namespace T.Issue.Commons.Extensions
{
    public static class EnumerableExtensions
	{
		/// <summary>
		/// Grazina eilute, kurioje reiksmes yra atskirtos kableliais.
		/// </summary>
		public static string ToCSV(this IEnumerable collection)
		{
			if (null == collection)
			{
				throw new InvalidOperationException("collection is null");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object current in collection)
			{
				stringBuilder.AppendFormat(",{0}", current);
			}
			string text = stringBuilder.ToString();
			if (text.StartsWith(","))
			{
				text = text.Substring(1);
			}
			return text;
		}
	}
}
