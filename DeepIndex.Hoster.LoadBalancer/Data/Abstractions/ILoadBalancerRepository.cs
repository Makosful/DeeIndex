namespace DeepIndex.Hoster.LoadBalancer.Data.Abstractions
{
    public interface ILoadBalancerRepository
    {
        public void QueueUp(string path);
        
        public string PopQueue();
    }
}