using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using DevBlog.Domain.Entities;

namespace DevBlog.Api.IntTests.Framework
{
    public static class TestData
    {
        public const string TestCollectionName = "ControllerCollection";
        private static readonly Random Random = new Random();

        public static string RandomEmail()
        {
            return RandomString(5) + "@" +  RandomString(5) + ".com";
        }


        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static List<Post> GetRandomPosts(int numberOfPosts)
        {
            var posts = new List<Post>();

            for (var count = 0; count < numberOfPosts; count++)
            {
                posts.Add(FakePost);
            }

            return posts;
        }

        public static Faker<Post> FakePost { get; } =
            new Faker<Post>()
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(6))
                .RuleFor(p => p.Slug, f => f.Lorem.Slug())
                .RuleFor(p => p.Body, f => f.Lorem.Paragraphs(8))
                .RuleFor(p => p.HeaderImage, f => f.Image.LoremFlickrUrl())
                .RuleFor(p => p.PublishDate, f => f.Date.Past())
                .RuleFor(p => p.ReadMinutes, f=>f.Random.Int(2, 15));
    }
}