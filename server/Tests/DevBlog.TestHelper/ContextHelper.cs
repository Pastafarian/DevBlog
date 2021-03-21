using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DevBlog.Domain;
using DevBlog.Domain.Entities;

namespace DevBlog.TestHelper
{
	public class ContextHelper
	{
		public Context Context { get; set; }

		public ContextHelper() { }

		public ContextHelper(Context context)
		{
			Context = context;
		}

		public ContextHelper BuildInMemoryHelper()
		{
			Context = null;
			return new ContextHelper(InMemoryContextBuilder.BuildInMemoryContext());
		}

		public ContextHelper BuildPgHelper()
		{
			Context = null;
			var optionsBuilder = new DbContextOptionsBuilder<Context>();
			var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

			if (string.IsNullOrEmpty(connectionString)) throw new Exception("Connection string null or empty.");

			optionsBuilder.UseNpgsql(connectionString);
			return new ContextHelper(new Context(optionsBuilder.Options));
		}

		public async Task CreatePost(Post post)
		{
			await Context.Posts.AddAsync(post);
			await Context.SaveChangesAsync();
		}
	}
}