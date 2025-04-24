using Microsoft.AspNetCore.Mvc;

namespace GameSubmission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpPost("exhello")]
        public IActionResult ExternalHelloWorld() 
        {
            var result = new { message = "Hello from the external API!" };
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Get() 
        {
            return Ok(new { message = "Game Submission API Test Message..." });
        }
    }
}

