using System.Collections.Generic;
using System.Linq;
using DeepIndex.Hoster.LoadBalancer.Models;


namespace DeepIndex.Hoster.LoadBalancer.Data
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(LoadBalancerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Jobs.Any())
            {
                return;   
            }

            List<Job> jobs = new List<Job>
            {
                new Job { Path = "test/2/3" },
                new Job { Path = "test/2/3/4/5" },
                new Job { Path = "test/2/3/4/5/6/7" }
            };

            context.Jobs.AddRange(jobs);
            context.SaveChanges();
        }
    }
}
