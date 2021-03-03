﻿using System.Threading.Tasks;
using DeepIndex.Core;
using DeepIndex.Infrastructure.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DeepIndex.Hoster.Crawler
{
    internal static class Program
    {
        /// <summary>
        /// Main method made to allow async method calls
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        /// <summary>
        /// Creates a template to build a Host from
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);

            host.ConfigureServices(ConfigureWorkers);
            host.UseSerilog();
            
            return host;
        }

        /// <summary>
        /// Uses Microsoft's Dependency Injection framework to add services to
        /// the application.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        private static void ConfigureWorkers(HostBuilderContext context, IServiceCollection services)
        {
            // Logging
            Log.Logger = ConfigureLogger(context.Configuration);
            services.AddLogging(x => x.AddSerilog());

            services.AddHostedService<Hoster.Crawler.Workers.Crawler>();
            services.AddCoreDependencies();
            services.AddSqliteDependencies();
        }

        /// <summary>
        /// Configures Serilog
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static ILogger ConfigureLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}