using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Application.Exp.Extensions;
using AuthApp.Application.Exp.Pipelines;
using AuthApp.Infra.Exp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthApp.Application.Exp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthAppContext>(
                options => options.UseNpgsql(
                    Configuration.GetConnectionString("AuthAppContext"),
                    b => b.MigrationsAssembly("AuthApp.Infra.Exp")
                )
            );

            // services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("ConString: " + Configuration.GetConnectionString("AuthAppConttext"));

            // UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // a way to run multiple independent ASP.NET Core pipelines side by side in the same application. This pipelines enable you to define your own services
            // for now, only one pipeline is used, but you are able to use multiple pipelines with different services
            app.UseBranchWithServices("/api", new ApiPipeline(Configuration));

            app.UseHttpsRedirection();

            /*still use the main branch normally if needed
            e.g. localhost:5000 – we do not enter any of our branches and “Not Found” prints cause we hit our middleware registered on the main branch*/
            app.Run(async c =>
            {
                c.Response.StatusCode = 404;
                await c.Response.WriteAsync("Not Found");
            });
        }

        // Automatic migration on application startup
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AuthAppContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

    }
}
