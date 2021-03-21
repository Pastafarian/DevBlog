using System;

namespace DevBlog.Application.Extensions
{
	public static class DateTimeExtensions
	{
		public static string ToDayWithFullMonthAndYear(this DateTime dateTime)
		{
			return AppendDaySuffix(dateTime.Day) + " " + dateTime.ToString("MMMM") + " " + dateTime.Year;
		}

		private static string AppendDaySuffix(int day)
		{
			string daySuffix;

			if (day != 1 && day != 21 && day != 31)
				if (day != 2 && day != 22)
					if (day == 3 || day == 23)
						daySuffix = "rd";
					else
						daySuffix = "th";
				else
					daySuffix = "nd";
			else
				daySuffix = "st";

			return day + daySuffix;
		}
	}
}
