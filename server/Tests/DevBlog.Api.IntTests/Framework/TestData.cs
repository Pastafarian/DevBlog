using System;
using System.Linq;

namespace DevBlog.Api.IntTests.Framework
{
    public static class TestData
    {
        public const string TestCollectionName = "ControllerCollection";
        public const string TestDbConnectionString = "Host=localhost;Port=5432;Username=UserName;Password=Password;Database=StephenAdamTestDb";
        private static readonly Random Random = new Random();

        public static string RandomEmail()
        {
            return RandomString(5) + "@" + RandomString(5) + ".com";
        }


        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}