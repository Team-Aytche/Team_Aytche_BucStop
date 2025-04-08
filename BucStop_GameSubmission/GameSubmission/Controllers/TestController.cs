using Microsoft.AspNetCore.Mvc;

namespace GameSubmission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get() 
        {
            return Ok(new { message = $"Game Submission API Test Message..." });
        }
    }
}
