using DevBlog.Application.Settings;
using DevBlog.Domain;
using Microsoft.AspNetCore.Builder;
using DevBlog.Application.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using DevBlog.Api.Middleware;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DevBlog.Api.Helpers
{
    public static class MiddlewareInitializer
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app, WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    if (!context.Response.HasStarted)
                        await context.Response.WriteAsync($"Unable to find end point. Url: {context.Request.GetDisplayUrl()}");
                }
            });

            // We might be able to get rid of this
            //https://stackoverflow.com/questions/52365434/linux-asp-net-core-with-apache-and-reverse-proxy
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<EnableRequestRewindMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseCors("Any");

            app.UseResponseCompression();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevBlog");
            });

            app.MapControllers();

            var appSettings = builder.Configuration.GetSection("AppSettings").GetSafely<AppSettings>();

            if (appSettings.RunMigrations)
            {
                using var scope = builder.Services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetService<Context>();

                if (dbContext != null && !dbContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
                {
                    dbContext.Database.Migrate();
                }
            }

            return app;
        }
    }
}
