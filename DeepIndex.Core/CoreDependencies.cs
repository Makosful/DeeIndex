using DeepIndex.Core.ApplicationServices;
using DeepIndex.Core.ApplicationServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DeepIndex.Core
{
    public static class CoreDependencies
    {
        public static void AddCoreDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOccurrenceService, OccurrenceService>();
        }
    }
}