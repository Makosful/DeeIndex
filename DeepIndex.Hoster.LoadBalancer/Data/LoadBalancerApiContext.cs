using DeepIndex.Hoster.LoadBalancer.Models;
using Microsoft.EntityFrameworkCore;

namespace DeepIndex.Hoster.LoadBalancer.Data
{
    public class LoadBalancerApiContext : DbContext
    {
        public LoadBalancerApiContext(DbContextOptions<LoadBalancerApiContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
    }
}
