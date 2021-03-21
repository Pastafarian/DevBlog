﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DevBlog.Domain.Factory
{
    public class BlogContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=UserName;Password=Password;Database=StephenAdam;");

            return new Context(optionsBuilder.Options);
        }
    }
}