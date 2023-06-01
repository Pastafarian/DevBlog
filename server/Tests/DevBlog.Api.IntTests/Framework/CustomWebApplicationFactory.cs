using System;
using System.Collections.Generic;
using DevBlog.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DevBlog.Api.IntTests.Framework;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _testContainers = new PostgreSqlBuilder()
        .WithDatabase("db")
        .WithPassword("postgres")
        .WithUsername("postgres")
        .WithImage("postgres:14.4")
        .Build();

    public IMessageSink Output;

    public CustomWebApplicationFactory(IMessageSink output)
    {
        Output = output;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _testContainers.StartAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(Context));
                    services.AddScoped(_ => new Context(new DbContextOptions<Context>(), new ConnectionString { AppConnectionString = _testContainers.GetConnectionString() }));
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<Context>();

                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();
                        db.Posts.AddRange(TestData.GetRandomPosts(10));
                        db.SaveChanges();
                    }

                    services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            SignatureValidator = (token, parameters) => new JwtSecurityToken(token),
                            ValidateAudience = false
                        };
                        options.Audience = TestAuthorisationConstants.Audience;
                        options.Authority = TestAuthorisationConstants.Issuer;
                        options.BackchannelHttpHandler = new MockBackchannel();
                        options.MetadataAddress = "https://inmemory.microsoft.com/common/.well-known/openid-configuration";
                    });
                }
        );


        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders(); // Remove other loggers
            logging.AddXUnit(Output); // Use the ITestOutputHelper instance
        });

        builder.ConfigureAppConfiguration(config =>
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("integrationsettings.json")
                .Build();

            config.AddConfiguration(configuration);
        });


        builder.UseEnvironment("Development");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("integrationsettings.json")
                .Build();

            config.AddConfiguration(configuration);
        });

        // This does not work.
        //builder.ConfigureAppConfiguration(config =>
        //{
        //    config.AddInMemoryCollection(new Dictionary<string, string> { { "Result", "Override" } });
        //});

        return base.CreateHost(builder);
    }

    public new async Task DisposeAsync()
    {
        await _testContainers.StopAsync();
    }
}