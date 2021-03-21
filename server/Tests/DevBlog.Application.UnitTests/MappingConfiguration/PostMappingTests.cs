using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using SimpleInjector;
using DevBlog.Application.Dtos;
using DevBlog.Application.MappingConfiguration;
using DevBlog.Domain.Entities;
using Xunit;

namespace DevBlog.Application.UnitTests.MappingConfiguration
{
    public class PostMappingTests
    {
        private readonly IMapper mapper;

        public PostMappingTests()
        {
            var container = new Container();
            mapper = GetMapper(container);
        }

        [Fact]
        public void GivenPostDto_WhenMappedToPostEntity_ThenPropertiesAreMapped()
        {
            // Arrange
            var postDto = new PostDto
            {
                Title = "here is a title",
                Body = "here is a body",
                HeaderImage = "foo.jpg",
                Slug = "some-slug"
            };

            // Act
            var post = mapper.Map<PostDto, Post>(postDto);

            // Assert
            Assert.Equal(post.Title, postDto.Title);
            Assert.Equal(post.Body, postDto.Body);
            Assert.Equal(post.HeaderImage, postDto.HeaderImage);
            Assert.Equal(post.Slug, postDto.Slug);
            Assert.Equal(1, post.ReadMinutes);
        }

        [Fact]
        public void GivenPostEntity_ThenMapsToPostSummaryDto()
        {
            // Arrange
            var post = new Post
            {
                Id = 2,
                Body = new string('a', 1000),
                HeaderImage = "header image",
                PublishDate = new DateTime(2019, 1, 1, 1, 1, 2),
                Slug = "article-slug",
                Title = "some title",
                ReadMinutes = 18
            };

            // Act
            var mapped = mapper.Map<List<Post>, List<PostDto>>(new List<Post> { post }).First();

            // Assert
            Assert.Equal(post.HeaderImage, mapped.HeaderImage);
            Assert.Equal(post.PublishDate, mapped.PublishDate);
            Assert.Equal(post.Slug, mapped.Slug);
            Assert.Equal(post.Title, mapped.Title);
            Assert.Equal(post.ReadMinutes, mapped.ReadMinutes);
            Assert.NotEmpty(mapped.Body);
        }

        [Fact]
        public void GivenListOfPosts_WhenMapped_ThenEachPostIsMapped()
        {
            // Arrange
            var posts = Enumerable.Repeat(new Post(), 20).ToList();

            // Act
            var result = mapper.Map<List<Post>, List<PostDto>>(posts);

            // Assert
            Assert.Equal(20, result.Count);
        }

        private static IMapper GetMapper(Container container)
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(container.GetInstance);

            mce.AddMaps(typeof(AutoMapperProfile).Assembly);

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            return new Mapper(mc, container.GetInstance);
        }
    }
}