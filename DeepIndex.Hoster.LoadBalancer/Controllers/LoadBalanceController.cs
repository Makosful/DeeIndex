using System;
using System.Collections.Generic;
using DeepIndex.Hoster.LoadBalancer.Data;
using DeepIndex.Hoster.LoadBalancer.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeepIndex.Hoster.LoadBalancer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadBalanceController : ControllerBase
    {
        private readonly IRepository<Job> _repository;

        public LoadBalanceController(IRepository<Job> repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var job = _repository.Get();
            if (job == null)
            {
                return NotFound();
            }
            _repository.Remove(job.Id);

            return new ObjectResult(job);
        }
        
        [HttpPost( Name = "GetJob")]
        public IActionResult Post([FromBody]Job job)
        {
            if (job == null)
            {
                return BadRequest();
            }

           
            var newJob = _repository.Add(job);
            return CreatedAtRoute("GetJob", new { id = newJob.Id,path = newJob.Path }, newJob);
        
        }
    }
}