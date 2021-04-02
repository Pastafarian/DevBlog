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
            optionsBuilder.UseNpgsql(TestValues.TestDbConnectionString);
			return new ContextHelper(new Context(optionsBuilder.Options));
		}

		public async Task CreatePost(Post post)
		{
			await Context.Posts.AddAsync(post);
			await Context.SaveChangesAsync();
		}
	}
}