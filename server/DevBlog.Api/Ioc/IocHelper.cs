using DevBlog.Api.Ioc.Registrations;
using DevBlog.Application.Settings;
using DevBlog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DevBlog.Application.MappingConfiguration;
using DevBlog.Application.Handlers.Query;
using DevBlog.Api.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json.Serialization;
using DevBlog.Application.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using SendGrid.Extensions.DependencyInjection;

namespace DevBlog.Api.Ioc
{
	public static class IocHelper
	{
        public static void Register(this IServiceCollection services, IConfiguration configurationManager)
        {
           services.AddLogging();
           services.AddControllers();
           services.AddMvcCore().AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
            });

            services.AddCors(x => x.AddPolicy("Any", y => { y.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
            var auth0Settings = configurationManager.GetSection("Auth0").GetSafely<Auth0Settings>();
            services.AddAuthenticationAndAuthorization(auth0Settings.Domain, auth0Settings.ApiIdentifier);
            services.AddResponseCompression();
            services.AddSwagger();
            
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;
            });

            var appSettings = configurationManager.GetSection("AppSettings").GetSafely<AppSettings>();

            services.AddSendGrid(options => { options.ApiKey = appSettings.SendGridApiKey; });
            var connectionString = new ConnectionString
            {
                AppConnectionString = configurationManager.GetConnectionString("DevBlog")
            };

            // We might be able to get rid of this
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddHttpContextAccessor();
            services.AddSingleton<EnableRequestRewindMiddleware>();
            services.AddSingleton<ExceptionHandlerMiddleware>();
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(GetPost.Query).Assembly));
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.RegisterServices();
            services.RegisterValidation();
            services.AddSingleton(appSettings);
            services.AddSingleton(connectionString);
            services.AddScoped(_ => new Context(new DbContextOptions<Context>(), connectionString));
            services.RegisterLogging();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dev Blog Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                c.CustomSchemaIds(x => x.FullName);
            });
        }

        public static void AddAuthenticationAndAuthorization(this IServiceCollection services, string domain,
            string audience)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            domain = $"https://{domain}/";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = audience;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin",
                    policy =>
                    {
                        policy.RequireClaim("http://stephenadam.api.io/isAdmin", "true");
                    });
            });
        }
    }
}
