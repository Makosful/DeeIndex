using System.Collections.Generic;
using System.Linq;
using DeepIndex.Hoster.LoadBalancer.Models;

namespace DeepIndex.Hoster.LoadBalancer.Data
{
    public class LoadBalanceRepository : IRepository<Job>
    {
        public readonly LoadBalancerApiContext Db;

        public LoadBalanceRepository(LoadBalancerApiContext db)
        {
            Db = db;
        }
        
        
        Job IRepository<Job>.Get()
        {
            return Db.Jobs.First();

        }
        
        void IRepository<Job>.Remove(int id)
        {
            var product = Db.Jobs.FirstOrDefault(p => p.Id == id);
            Db.Jobs.Remove(product);
            Db.SaveChanges();
        }
        Job IRepository<Job>.Add(Job entity)
        {
            var newJob = Db.Jobs.Add(entity).Entity;
            Db.SaveChanges();
            return newJob;
        }
        
    }
}