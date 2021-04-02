using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using DevBlog.Api.Ioc;
using DevBlog.Application.Settings;
using DevBlog.Domain;
using DevBlog.TestHelper;

namespace DevBlog.Api.IntTests.Framework
{
    public class ControllerFixture : IDisposable
    {
        public IMediator Mediator { get; private set; }
        public Context Context { get; }

        public ControllerFixture()
        {
            var container = IocHelper.BuildContainer(Lifestyle.Singleton);
            container.Register(new AppSettings(), new LoggerSettings());

            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseNpgsql(TestValues.TestDbConnectionString);
            var context = new Context(optionsBuilder.Options);
            context.Database.EnsureCreated();
            container.Register(()=> context);
            

            Mediator = container.GetInstance<IMediator>();
            Context = context;
        }

        public void Dispose() 
        {
        }
    }
}