using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace BucStop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "game_submissions_api.json");
        private readonly string _helloFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "hello_world.json");

        // --- Keep this! Just for testing ---
        [HttpPost("hello")]
        public IActionResult HelloWorld()
        {
            var content = new { message = "Hello World!" };
            var json = JsonSerializer.Serialize(content);
            System.IO.File.WriteAllText(_helloFilePath, json);
            return Ok(new { status = "success", message = "Hello World written to file." });
        }

        // --- Main game submission endpoint ---
        [HttpPost]
        public IActionResult SubmitGame([FromBody] GameSubmission submission)
        {
            if (submission == null ||
                string.IsNullOrWhiteSpace(submission.Name) ||
                string.IsNullOrWhiteSpace(submission.GameTitle) ||
                string.IsNullOrWhiteSpace(submission.GameDescrip) ||
                string.IsNullOrWhiteSpace(submission.HowToPlay) ||
                string.IsNullOrWhiteSpace(submission.ETSUEmailPrefix) ||
                string.IsNullOrWhiteSpace(submission.GitHubLink))
            {
                return BadRequest("Missing required fields.");
            }

            // Add email suffix if needed
            if (!submission.ETSUEmailPrefix.EndsWith("@etsu.edu"))
            {
                submission.ETSUEmailPrefix += "@etsu.edu";
            }

            // Read existing or create new
            List<GameSubmission> submissions = new();
            if (System.IO.File.Exists(_jsonFilePath))
            {
                var existingJson = System.IO.File.ReadAllText(_jsonFilePath);
                submissions = JsonSerializer.Deserialize<List<GameSubmission>>(existingJson) ?? new List<GameSubmission>();
            }

            submissions.Add(submission);

            // Save it
            var updatedJson = JsonSerializer.Serialize(submissions, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_jsonFilePath, updatedJson);

            return Ok(new { status = "success", message = "Game submission saved." });
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
                    var gameInfoList = await response.Content.ReadFromJsonAsync<List<GameInfo>>();
                    return Ok(gameInfoList);
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

    // GameSubmission model
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
