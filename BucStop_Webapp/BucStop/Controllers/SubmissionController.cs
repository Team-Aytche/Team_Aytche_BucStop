using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Collections.Generic;

namespace BucStop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ILogger<SubmissionController> _logger; //Added for logging
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "game_submissions_api.json");
        private readonly string _helloFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "hello_world.json");


        //Inject Logger
        public SubmissionController(ILogger<SubmissionController> logger)
        {
        _logger = logger;
        }

        // --- Keep this! Just for testing ---
        [HttpPost("hello")]
        public IActionResult HelloWorld()
        {
            _logger.LogInformation("HelloWorld endpoint hit."); //Log
            var content = new { message = "Hello World!" };
            var json = JsonSerializer.Serialize(content);
            System.IO.File.WriteAllText(_helloFilePath, json);
            _logger.LogInformation("Hello World written to file."); //Logged
            return Ok(new { status = "success", message = "Hello World written to file." });
        }

        // --- Main game submission endpoint ---
        [HttpPost]
        public IActionResult SubmitGame([FromBody] GameSubmission submission)
        {
            _logger.LogInformation("Received Game Submission"); //Log Game submission
            if (submission == null ||
                string.IsNullOrWhiteSpace(submission.Name) ||
                string.IsNullOrWhiteSpace(submission.GameTitle) ||
                string.IsNullOrWhiteSpace(submission.GameDescrip) ||
                string.IsNullOrWhiteSpace(submission.HowToPlay) ||
                string.IsNullOrWhiteSpace(submission.ETSUEmailPrefix) ||
                string.IsNullOrWhiteSpace(submission.GitHubLink))
            {
                _logger.LogWarning("Submission Failed: missing required fields"); //Log the failed request
                return BadRequest("Missing required fields.");
            }

            // Add email suffix if needed
            if (!submission.ETSUEmailPrefix.EndsWith("@etsu.edu"))
            {
                submission.ETSUEmailPrefix += "@etsu.edu";
            }

            // Read existing or create new
            List<GameSubmission> submissions = new();
            try
            {
                if (System.IO.File.Exists(_jsonFilePath))
                {
                    var existingJson = System.IO.File.ReadAllText(_jsonFilePath);
                    submissions = JsonSerializer.Deserialize<List<GameSubmission>>(existingJson) ?? new List<GameSubmission>();
                }
    
                submissions.Add(submission);
    
                // Save it
                var updatedJson = JsonSerializer.Serialize(submissions, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_jsonFilePath, updatedJson);
                _logger.LogInformation("Successfully wrote new submission to file");
    
                return Ok(new { status = "success", message = "Game submission saved." });
            }
            
            catch (exception ex)
            {
                _logger.LogError(ex, "Failed to write submission to file"); //Log
                return StatusCode(500, "Innternal server error"); //

            }
        }
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
