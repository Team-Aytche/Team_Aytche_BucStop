using Microsoft.AspNetCore.Mvc;
using BucStop.Models;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BucStop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameService _gameService;

        //Path to store game submissions in JSON format
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "game_submissions.json");

        public HomeController(ILogger<HomeController> logger, GameService games)
        {
            _logger = logger;
            _gameService = games;
        }

        //Sends the user to the deprecated Index page.
        public IActionResult Index()
        {
            return View(_gameService.GetGames());
        }

        //Takes the user to the admin page.
        public IActionResult Admin()
        {
            return View();
        }

        //Takes the user to the about policy page.
        public IActionResult Privacy()
        {
            return View();
        }

        //Takes the user to the game criteria page.
        public IActionResult GameCriteria()
        {
            return View();
        }

        //Takes the user to version 2.1 page
        public IActionResult TwoDotOne()
        {
            return View();
        }

        public IActionResult TwoDotTwo()
        {
            return View();
        }

        public IActionResult TwoDotThree()
        {
            return View();
        }

        public IActionResult TwoDotFour()
        {
            return View();
        }

        //If something goes wrong, this will take the user to a page explaining the error.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Action for submitting a game
        [HttpPost]
        public IActionResult SubmitGame(string name, string gameTitle, string gameDescrip, string howToPlay, string ETSUEmailPrefix, string GitHubLink)
        {
            //Validate the data
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(gameTitle) || string.IsNullOrWhiteSpace(gameDescrip) || string.IsNullOrWhiteSpace(howToPlay) || string.IsNullOrWhiteSpace(ETSUEmailPrefix) || string.IsNullOrWhiteSpace(GitHubLink))
            {
                return BadRequest("Please fill in all required fields.");
            }

            //Append "@etsu.edu" to the ETSU Email Prefix
            if (!string.IsNullOrEmpty(ETSUEmailPrefix))
            {
                ETSUEmailPrefix += "@etsu.edu";
            }

            //Create a new game submission object
            var newSubmission = new GameSubmission
            {
                Name = name,
                GameTitle = gameTitle,
                GameDescrip = gameDescrip,
                HowToPlay = howToPlay,
                ETSUEmailPrefix = ETSUEmailPrefix,
                GitHubLink = GitHubLink
            };

            //Get the existing game submissions
            var gameSubmissions = GetGameSubmissions();

            //Add the new submission
            gameSubmissions.Add(newSubmission);

            //Save all game submissions back to the JSON file
            SaveGameSubmissions(gameSubmissions);

            //Return a success message
            return Ok("Submission successful!");
        }

        //Get existing game submissions from the JSON file
        private List<GameSubmission> GetGameSubmissions()
        {
            if (!System.IO.File.Exists(_jsonFilePath))
            {
                return new List<GameSubmission>(); //Return an empty list if the file doesn't exist
            }

            //Read the content of the JSON file
            var json = System.IO.File.ReadAllText(_jsonFilePath);

            //Deserialize the JSON content into a list of GameSubmission objects
            return JsonSerializer.Deserialize<List<GameSubmission>>(json) ?? new List<GameSubmission>();
        }

        //Save the list of game submissions to the JSON file
        private void SaveGameSubmissions(List<GameSubmission> gameSubmissions)
        {
            //Serialize the list of game submissions to JSON
            var jsonData = JsonSerializer.Serialize(gameSubmissions);

            //Write the JSON data to the file
            System.IO.File.WriteAllText(_jsonFilePath, jsonData);
        }

        //New method to handle saving a single game submission (if required)
        public IActionResult SaveGameSubmission(GameSubmission submission)
        {
            //Ensure ETSUEmailPrefix is properly set with the @etsu.edu suffix
            if (!string.IsNullOrEmpty(submission.ETSUEmailPrefix))
            {
                submission.ETSUEmailPrefix += "@etsu.edu";
            }

            //Serialize the data to JSON
            var jsonData = JsonSerializer.Serialize(submission);

            //Save the serialized JSON data to a file (in the current directory)
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "game_submission.json");
            System.IO.File.WriteAllText(filePath, jsonData);

            //Return some response or redirect as necessary
            return View();
        }
    }

    //Define the structure for the game submission data
    public class GameSubmission
    {
        public string Name { get; set; } = string.Empty; // Initialize with an empty string
        public string GameTitle { get; set; } = string.Empty;
        public string GameDescrip { get; set; } = string.Empty;
        public string HowToPlay { get; set; } = string.Empty;
        public string ETSUEmailPrefix { get; set; } = string.Empty;
        public string GitHubLink { get; set; } = string.Empty;
    }
}
