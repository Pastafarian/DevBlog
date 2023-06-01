using DevBlog.Api.Helpers;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using DevBlog.Api.Ioc;

var configuration = ConfigurationHelper.Build();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Register(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware(builder);


try
{
    logger.Information("Starting web host");
    Log.Information("Starting web host from another logger");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Information("Exception Here");
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
