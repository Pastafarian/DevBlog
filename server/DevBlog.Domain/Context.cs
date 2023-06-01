#nullable enable
using Microsoft.EntityFrameworkCore;
using DevBlog.Domain.Entities;

namespace DevBlog.Domain
{
#pragma warning disable 8618
	public class Context : DbContext
	{
        private readonly ConnectionString _connectionString;

        public Context(DbContextOptions<Context> options, ConnectionString connectionString) : base(options)
        {
            _connectionString = connectionString;
        }

		public DbSet<Post> Posts { get; set; }

		public DbSet<Contact> Contacts { get; set; }

		public DbSet<File> File { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString.AppConnectionString);
        }
	}
}
