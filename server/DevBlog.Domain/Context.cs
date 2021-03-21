#nullable enable
using Microsoft.EntityFrameworkCore;
using DevBlog.Domain.Entities;
using System;

namespace DevBlog.Domain
{
#pragma warning disable 8618
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options) : base(options)
		{
		}

		public DbSet<Post> Posts { get; set; }

		public DbSet<Contact> Contacts { get; set; }

		public DbSet<File> File { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Post>().HasData(
				new
				{
					Id = 1,
					ReadMinutes = 8,
					Title = "Angular",
					Slug = "angular",
					Body = "Hello world",
					Content = "Test 1",
					PublishDate = DateTime.UtcNow,
					HeaderImage = "angular-card2.png"
				},
				new
				{
					Id = 2,
					ReadMinutes = 6,
					Title = ".NET Core",
					Slug = "core",
					Body = "Hello world",
					Content = "Test 2",
					PublishDate = DateTime.UtcNow,
					HeaderImage = "dotnet-card2.png"
				},
				new
				{
					Id = 3,
					ReadMinutes = 9,
					Title = "Git Hub",
					Slug = "git",
					Body = "Hello world",
					Content = "Test 2",
					PublishDate = DateTime.UtcNow,
					HeaderImage = "code-card3.png"
				});

			base.OnModelCreating(builder);
		}
	}
}
