using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeepIndex.Core.ApplicationServices.Abstractions;
using DeepIndex.Core.Entities;
using DeepIndex.Core;
using DeepIndex.Infrastructure.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DeepIndex.Ui.Cli
{
    internal class Program
    {
        private readonly IOccurrenceService _occurrence;
        private readonly IConfiguration _configuration;
        public Program(IOccurrenceService occurrence, IConfiguration configuration)
        {
            _occurrence = occurrence;
            _configuration = configuration;
        }
        
        private static void Main(string[] args)
        {
            string? env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("appsettings.json", optional: true);
            if (env != null) configBuilder.AddJsonFile($"appsettings.{env}.json", optional: true);
            configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            configBuilder.AddEnvironmentVariables();
            configBuilder.AddCommandLine(args);
            IConfiguration configuration = configBuilder.Build(); 
                
            IServiceCollection services = new ServiceCollection();
            
            // Logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            services.AddLogging(builder => builder.AddSerilog());

            services.AddSingleton<Program>();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddCoreDependencies();
            services.AddSqliteDependencies();
            
            services.BuildServiceProvider().GetService<Program>().Run();
        }

        private void Run()
        {
            Console.Write("Enter the search term: ");
            string input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(input)) return;

            List<Occurrence> occurrences = _occurrence.SearchTerm(input).ToList();
            Console.WriteLine(occurrences.Count);

            foreach (Occurrence occurrence in occurrences)
            {
                Console.WriteLine(occurrence.File);
            }
        }
    }
}