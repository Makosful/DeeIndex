using DeepIndex.Hoster.LoadBalancer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DeepIndex.Hoster.LoadBalancer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadBalanceController : ControllerBase
    {
        private readonly ILoadBalancerRepository _repository;

        public LoadBalanceController(ILoadBalancerRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post([FromBody]string[] paths)
        {
            if (paths == null || paths.Length < 1)
            {
                return BadRequest();
            }

            foreach (var path in paths)
            {
                _repository.QueueUp(path);
            }
            
            return Ok();
        }
    }
}