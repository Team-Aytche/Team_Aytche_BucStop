using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GameSubmission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "received_submissions.json");

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

        [HttpPost("submit-external")]
        public async Task<IActionResult> ReceiveGameSubmission([FromBody] GameSubmission submission)
        {
            if (submission == null)
                return BadRequest("Submission was empty.");

            // Ensure Data directory exists
            var dataDir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            // Load existing submissions
            List<GameSubmission> submissions = new();
            if (System.IO.File.Exists(_filePath))
            {
                var existing = await System.IO.File.ReadAllTextAsync(_filePath);
                submissions = JsonSerializer.Deserialize<List<GameSubmission>>(existing) ?? new List<GameSubmission>();
            }

            submissions.Add(submission);

            // Save updated list
            var json = JsonSerializer.Serialize(submissions, new JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(_filePath, json);

            return Ok(new { status = "success", message = "Submission received and stored." });
        }

    }


    public class GameSubmission
    {
        public string Name { get; set; } = string.Empty;
        public string GameTitle { get; set; } = string.Empty;
        public string GameDescrip { get; set; } = string.Empty;
        public string HowToPlay { get; set; } = string.Empty;
        public string ETSUEmailPrefix { get; set; } = string.Empty;
        public string GitHubLink { get; set; } = string.Empty;
    }
}

