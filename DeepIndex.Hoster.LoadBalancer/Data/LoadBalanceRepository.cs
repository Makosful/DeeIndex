using System.Collections;
using DeepIndex.Hoster.LoadBalancer.Data.Abstractions;

namespace DeepIndex.Hoster.LoadBalancer.Data
{
    public class LoadBalanceRepository : ILoadBalancerRepository
    {
        private readonly Queue _queue;

        public LoadBalanceRepository()
        {
            var queue = new Queue();
            _queue = Queue.Synchronized(queue);
        }

        public void QueueUp(string path)
        {
            _queue.Enqueue(path);
        }

        public string PopQueue()
        {
            var path = _queue.Dequeue() as string;
            return path;
        }
    }
}