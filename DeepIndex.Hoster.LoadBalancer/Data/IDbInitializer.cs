namespace DeepIndex.Hoster.LoadBalancer.Data
{
    public interface IDbInitializer
    {
        void Initialize(LoadBalancerApiContext context);
    }
}
