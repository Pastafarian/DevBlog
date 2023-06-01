using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DevBlog.Domain.Factory
{
	public class BlogContextFactory : IDesignTimeDbContextFactory<Context>
	{
        private readonly ConnectionString _connectionString;

        public BlogContextFactory(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

		public Context CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<Context>();

            return new Context(optionsBuilder.Options, _connectionString);
		}
	}
}