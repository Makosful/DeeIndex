using Microsoft.AspNetCore.Mvc;

namespace DeepIndex.Hoster.Indexer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndexController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}