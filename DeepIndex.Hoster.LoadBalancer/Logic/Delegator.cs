using System;
using System.Threading.Tasks;
using DeepIndex.Hoster.LoadBalancer.Data;
using DeepIndex.Hoster.LoadBalancer.Logic.Abstractions;
using DeepIndex.Hoster.LoadBalancer.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace DeepIndex.Hoster.LoadBalancer.Logic
{
    public class Delegator : IDeligator
    {
        private readonly IRepository<Job> _repository;
        private readonly IConfiguration _configuration;

        public Delegator(IRepository<Job> repository, IConfiguration config)
        {
            _repository = repository;
            _configuration = config;
        }

        public bool SendFilePath(string IP)
        {
            RestClient client = new RestClient(IP);
            
            Job job = _repository.Get();
            var request = new RestRequest("path")
                .AddJsonBody(job.Path);
                
            IRestResponse response = client.Post(request);

            return response.IsSuccessful;
        }
        
        
        
        public void Start()
        {
            string[] workerIPs = _configuration.GetValue<string[]>("Indexers");
            
            foreach (string ip in workerIPs)
            {
                SendFilePath(ip);
            }
        }
        
    }
}