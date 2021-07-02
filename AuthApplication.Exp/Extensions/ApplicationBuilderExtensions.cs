using AuthApp.Application.Exp.Pipelines;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AuthApp.Application.Exp.Extensions
{
    /*TypedControllerFeatureProvider is typed against TController. The generic is then used as a part of base type verification logic
    so that we can decide that a controller is valid by checking if it inherits from a relevant base type.*/
    public class TypedControllerFeatureProvider<TController> : ControllerFeatureProvider where TController : ControllerBase
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (!typeof(TController).GetTypeInfo().IsAssignableFrom(typeInfo)) return false;
            return base.IsController(typeInfo);
        }
    }

    public static class ApplicationBuilderExtensions
    {
        // adding UseBranchWithServices in IApplicationBuilder, parameter with "this" point to the one who call this method
        public static IApplicationBuilder UseBranchWithServices(this IApplicationBuilder app,
            PathString path,
            IPipeline pipeline)
        {
            /* create an IWebHost instance to obtain a “clean”, “independent” IServiceProvider
            use pipeline to set up any services needed on this branch
            with defining the interface, the method of ConfigureServices can be called here*/
            var webHost = new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(pipeline.ConfigureServices)
                //.ConfigureLogging()
                .UseStartup<EmptyStartup>()
                .Build();
            var serviceProvider = webHost.Services;
            var serverFeatures = webHost.ServerFeatures;

            var appBuilderFactory = serviceProvider.GetRequiredService<IApplicationBuilderFactory>();
            var branchBuilder = appBuilderFactory.CreateBuilder(serverFeatures);
            var factory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            branchBuilder.Use(async (context, next) =>
            {
                using (var scope = factory.CreateScope())
                {
                    context.RequestServices = scope.ServiceProvider;
                    await next();
                }
            });

            pipeline.ConfigureAppBuilder(branchBuilder);

            var branchDelegate = branchBuilder.Build();

            return app.Map(path, builder =>
            {
                builder.Use(async (context, next) =>
                {
                    await branchDelegate(context);
                });
            });
        }

        private class EmptyStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure(IApplicationBuilder app)
            {
            }
        }
    }
}