using System;
using System.Text.RegularExpressions;

namespace DevBlog.TestHelper
{
	public static class TestValues
	{
        public const string TestDbConnectionString = "Host=localhost;Port=5432;Username=a;Password=a;Database=DevBlogTestDb";

		public static string UniqueEmail()
		{
			return (UniqueString(5) + "@" + UniqueString(5) + ".com").ToLower();
		}

		public static string UniqueString(int length)
		{
			var s = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
			return length == 0 ? s : s.Substring(0, length);
		}

		public static string UniquePassword()
		{
			var s = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
			return s.Substring(0, 6) + "1tT*";
		}

		public static bool RandomBool()
		{
			return new Random().Next(0, 2) == 1;
		}

		public static DateTime DateInPast()
		{
			return DateTime.Now.AddYears(-1);
		}

		public static DateTime DateInFuture()
		{
			return DateTime.Now.AddYears(1);
		}
	}
}