using DeepIndex.Core.DomainServices;
using DeepIndex.Infrastructure.Sqlite.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace DeepIndex.Infrastructure.Sqlite
{
    public static class SqliteDependencies
    {
        public static void AddSqliteDependencies(this IServiceCollection services)
        {
            services.AddDbContext<IndexContext>();

            services.AddScoped<IOccurrenceDomain, OccurrenceDomain>();
        }
    }
}