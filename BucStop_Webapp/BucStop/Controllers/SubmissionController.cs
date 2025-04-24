using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net.Http;

namespace BucStop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ILogger<SubmissionController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "game_submissions_api.json");
        private readonly string _helloFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "hello_world.json");

        // Constructor with both Logger and HttpClientFactory injected
        public SubmissionController(ILogger<SubmissionController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("external-hello")]
        public async Task<IActionResult> CallExternalHello()
        {
            var client = _httpClientFactory.CreateClient();
            var url = "http://localhost:7777/api/Test/exhello";

            try 
            {
                var response = await client.PostAsync(url, null);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to call external Hello World.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var externalHelloResponsePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "external_hello_response.json");
                System.IO.File.WriteAllText(externalHelloResponsePath, content);

                return Ok(new { status = "success", data = content });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("hello")]
        public IActionResult HelloWorld()
        {
            _logger.LogInformation("HelloWorld endpoint hit.");
            var content = new { message = "Hello World!" };
            var json = JsonSerializer.Serialize(content);
            System.IO.File.WriteAllText(_helloFilePath, json);
            _logger.LogInformation("Hello World written to file.");
            return Ok(new { status = "success", message = "Hello World written to file." });
        }

        [HttpPost]
        public IActionResult SubmitGame([FromBody] GameSubmission submission)
        {
            _logger.LogInformation("Received Game Submission");

            if (submission == null ||
                string.IsNullOrWhiteSpace(submission.Name) ||
                string.IsNullOrWhiteSpace(submission.GameTitle) ||
                string.IsNullOrWhiteSpace(submission.GameDescrip) ||
                string.IsNullOrWhiteSpace(submission.HowToPlay) ||
                string.IsNullOrWhiteSpace(submission.ETSUEmailPrefix) ||
                string.IsNullOrWhiteSpace(submission.GitHubLink))
            {
                _logger.LogWarning("Submission Failed: missing required fields");
                return BadRequest("Missing required fields.");
            }

            if (!submission.ETSUEmailPrefix.EndsWith("@etsu.edu"))
            {
                submission.ETSUEmailPrefix += "@etsu.edu";
            }

            List<GameSubmission> submissions = new();
            try
            {
                if (System.IO.File.Exists(_jsonFilePath))
                {
                    var existingJson = System.IO.File.ReadAllText(_jsonFilePath);
                    submissions = JsonSerializer.Deserialize<List<GameSubmission>>(existingJson) ?? new List<GameSubmission>();
                }

                submissions.Add(submission);
                var updatedJson = JsonSerializer.Serialize(submissions, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_jsonFilePath, updatedJson);
                _logger.LogInformation("Successfully wrote new submission to file");

                return Ok(new { status = "success", message = "Game submission saved." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write submission to file");
                return StatusCode(500, "Internal server error");
            }
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
