using AuthApp.Application.Controllers;
using AuthApp.Application.Exp.Extensions;
using AuthApp.Application.Exp.Interfaces;
using AuthApp.Application.Exp.Middleware;
using AuthApp.Application.Exp.Services;
using AuthApp.Infra.Exp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthApp.Application.Exp.Pipelines
{
    public class ApiPipeline : IPipeline
    {
        private IConfiguration configuration;

        public ApiPipeline(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureAppBuilder(IApplicationBuilder app)
        {

            app.UseCors("CorsPolicy");

            app.UseRouting();

            // Auth middleware
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<UserCheckMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var managementTokenOptions = new Auth0MachineTokenFactoryOptions()
            {
                Domain = configuration["Auth0ManagementAPIService:Domain"],
                Audience = configuration["Auth0ManagementAPIService:Audience"],
                ManagementClientId = configuration["Auth0ManagementAPIService:ManagementClientId"],
                ManagementClientSecret = configuration["Auth0ManagementAPIService:ManagementClientSecret"],
                JwtExpirationInSeconds = 60 * 60 * 5 // 5 hours
            };

            services.AddSingleton<ManagementTokenFactory>((options) => new ManagementTokenFactory(managementTokenOptions));
            services.AddSingleton<Auth0Service>();
            services.AddSingleton<IBookService, BookService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddLogging((c) =>
            {
                c.AddConfiguration(configuration.GetSection("Logging.LogLevel"));
                c.AddConsole();
            });

            services.AddControllers().ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Clear();
                manager.FeatureProviders.Add(new TypedControllerFeatureProvider<ApiControllerBase>());
            });

            services.AddDbContext<AuthAppContext>(options =>
                options
                    .UseNpgsql(
                        configuration.GetConnectionString("AuthAppContext"),
                        b => b.MigrationsAssembly("AuthApp.Infra.Exp")
                    )
            );

            //

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    );
            });

            //

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{configuration["Auth0:Domain"]}/";
                options.Audience = configuration["Auth0:Audience"];
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = (context) =>
                    {
                        // access token coming with header is changed to securityToken object after being successfully parsed
                        var accessToken = context.SecurityToken as JwtSecurityToken;
                        if (accessToken != null)
                        {
                            var identity = context.Principal.Identity as ClaimsIdentity;
                            if (identity != null)
                            {
                                identity.AddClaim(new Claim("access_token", accessToken.RawData));
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidateAudience = true,
                    ValidAudience = configuration["Auth0:Audience"]
                };
            });

        }
    }
}