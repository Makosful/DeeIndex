using DeepIndex.Hoster.Indexer.Data;
using DeepIndex.Hoster.Indexer.Data.Abstractions;
using DeepIndex.Hoster.Indexer.Logic;
using DeepIndex.Hoster.Indexer.Logic.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeepIndex.Hoster.Indexer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<ICrawler, Crawler>();
            services.AddScoped<IRestAccess, RestAccess>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}