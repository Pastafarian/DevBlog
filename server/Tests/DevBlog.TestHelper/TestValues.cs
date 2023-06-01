using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DevBlog.TestHelper
{
	public static class TestValues
	{

		public static string UniqueEmail()
		{
			return (UniqueString(5) + "@" + UniqueString(5) + ".com").ToLower();
		}

		public static string UniqueString(int length)
		{
			var s = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
			return length == 0 ? s : s[..length];
		}

        public static string UniqueUrl()
        {
            return "https://www." + (UniqueString(5) + ".com/" + UniqueString(5)).ToLower();
        }

        public static string UniquePassword()
		{
			var s = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
			return s[..6] + "1tT*";
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

        public static ByteArrayContent SerializeObjectToByteArrayContent<TValue>(TValue obj)
        {
            var json = JsonSerializer.Serialize(obj);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}