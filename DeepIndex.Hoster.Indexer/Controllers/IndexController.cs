using DeepIndex.Hoster.Indexer.Logic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DeepIndex.Hoster.Indexer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndexController : Controller
    {
        private readonly ICrawler _crawler;

        public IndexController(ICrawler crawler)
        {
            _crawler = crawler;
        }

        [HttpPost]
        public IActionResult Index([FromBody] string path)
        {
            bool isCrawled = _crawler.CrawlFile(path);
            
            if (!isCrawled)
                return Problem();
            
            return Ok();
        }
    }
}