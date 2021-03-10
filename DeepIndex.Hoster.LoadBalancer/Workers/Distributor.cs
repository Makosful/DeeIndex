using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeepIndex.Hoster.LoadBalancer.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace DeepIndex.Hoster.LoadBalancer.Workers
{
    public class Distributor : BackgroundService
    {
        private readonly ILogger<Distributor> _logger;
        
        private readonly ILoadBalancerRepository _repository;
        private readonly IConfiguration _configuration;

        private Timer _timer;

        public Distributor(ILoadBalancerRepository repository, IConfiguration configuration, ILogger<Distributor> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
        }

        public CancellationToken Token { get; set; }
        
        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Token = stoppingToken;
            _logger.LogInformation("Initializing tasks");
            IConfigurationSection[] hosts = _configuration.GetSection("Indexers").GetChildren().ToArray();

            var tasks = new List<Task>();
            foreach (var host in hosts)
            {
                tasks.Add(CreateTask(host.Value, stoppingToken));
            }

            _timer = new Timer(DoWork, tasks, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (!(state is List<Task> tasks)) return;

            for (var index = 0; index < tasks.Count; index++)
            {
                Task task = tasks[index];
                var host = task.AsyncState as string;

                _logger.LogInformation("{Time} - Host [{Host}] is [{Status}]", DateTime.Now, host, task.Status);
                if (task.Status == TaskStatus.Running)
                    continue;

                _logger.LogInformation("{Time} - Rebuilding task for host [{Host}]", DateTime.Now, host);
                tasks[index] = CreateTask(host, Token);

                tasks[index].Start();
            }
        }

        private Task CreateTask(string host, CancellationToken stoppingToken)
        {
            var task = new Task(ConnectRest, host, stoppingToken, TaskCreationOptions.AttachedToParent);
            return task;
        }

        private void ConnectRest(object section)
        {
            var host = section as string;
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException();

            var client = new RestClient(host) {Timeout = 60 * 10}; // 60 seconds * 10 minutes
            var request = new RestRequest("index");
            
            while (true)
            {
                try
                {
                    string path = _repository.PopQueue();
                    request.AddJsonBody(path);

                }
                catch (InvalidOperationException)
                {
                    //return;
                }
                finally
                {
                    Random r = new Random();
                    TimeSpan seconds = TimeSpan.FromSeconds(r.Next(5, 20));
                    Thread.Sleep(seconds);
                }
                return;

                IRestResponse response = client.Post(request);

                if (response.IsSuccessful)
                {
                    // Successful
                    // Do nothing for now
                }
                else
                {
                    // Not successful
                    // Do nothing for now
                }
            }
        }
    }
}