using System.Threading.Tasks;
using DevBlog.Domain;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace DevBlog.Application.IntTests;

public class ContextBuilder : IAsyncLifetime
{
    public Context Context;

    private readonly PostgreSqlContainer _testContainers = new PostgreSqlBuilder()
        .WithDatabase("db")
        .WithPassword("postgres")
        .WithUsername("postgres")
        .WithImage("postgres:14.4")
        .Build();

    public void Build()
    {
        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        optionsBuilder.UseNpgsql(_testContainers.GetConnectionString());
        Context = new Context(optionsBuilder.Options,
            new ConnectionString { AppConnectionString = _testContainers.GetConnectionString() });
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public async Task InitializeAsync()
    {
        await _testContainers.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _testContainers.StopAsync();
    }
}