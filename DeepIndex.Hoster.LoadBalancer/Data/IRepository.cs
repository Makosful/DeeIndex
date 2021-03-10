using System.Collections.Generic;

namespace DeepIndex.Hoster.LoadBalancer.Data
{
    public interface IRepository<T>
    {
        T Get();
        T Add(T entity);
        void Remove(int id);
    }
}