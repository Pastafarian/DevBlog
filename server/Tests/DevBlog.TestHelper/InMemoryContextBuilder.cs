using Microsoft.EntityFrameworkCore;
using DevBlog.Domain;

namespace DevBlog.TestHelper
{
	public static class InMemoryContextBuilder
	{
		public static Context BuildInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<Context>()
				.UseInMemoryDatabase("UserDB")
				.Options;

			var context = new Context(options);
			context.Database.EnsureCreated();
			context.Database.EnsureDeleted();
			context.SaveChanges();
			return context;
		}
	}
}