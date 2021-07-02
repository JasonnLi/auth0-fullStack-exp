using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApp.Application.Exp.Pipelines
{
    public interface IPipeline
    {
        void ConfigureServices(IServiceCollection services);

        void ConfigureAppBuilder(IApplicationBuilder app);
    }
}