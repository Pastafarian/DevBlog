using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DevBlog.Application.Dtos;
using DevBlog.Application.MappingConfiguration;
using DevBlog.Domain.Entities;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace DevBlog.Application.UnitTests.MappingConfiguration
{
    public class PostMappingTests
    {
        private readonly IMapper _mapper;

        public PostMappingTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            _mapper = services.BuildServiceProvider().GetService<IMapper>();
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
            var post = _mapper.Map<PostDto, Post>(postDto);

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
            var mapped = _mapper.Map<List<Post>, List<PostDto>>(new List<Post> { post }).First();

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
            var result = _mapper.Map<List<Post>, List<PostDto>>(posts);

            // Assert
            Assert.Equal(20, result.Count);
        }
    }
}