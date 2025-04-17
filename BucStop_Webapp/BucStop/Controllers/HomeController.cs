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
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "game_submissions.json");


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
    }
}
