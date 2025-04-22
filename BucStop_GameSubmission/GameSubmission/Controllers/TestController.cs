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

            return Ok(new { message =  });
        }
    }


      [HttpGet("expayload")]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Example: change base URL and endpoint as needed
                var baseUrl = "http://localhost:7777/";
                var endpoint = "api/Test/expayload";

                _httpClient.BaseAddress = new Uri(baseUrl);
                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var gameInfoList = await response.Content.ReadAsStringAsync();

                    var externalHelloResponsePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "external_hello_response.json");
                    System.IO.File.WriteAllText(externalHelloResponsePath, gameInfoList);

                    return Ok("Success");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to get data from {baseUrl}{endpoint}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    // Add this class or import it from your shared project
    public class GameInfo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string HowTo { get; set; }
        public Stack<KeyValuePair<string, int>> LeaderBoardStack { get; set; }
    }
}
