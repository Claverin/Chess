using Chess.Data;
using Chess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGameService _gameService;
        private readonly MongoDbService _mongoDbService;

        public GameController(
            ILogger<GameController> logger,
            UserManager<ApplicationUser> userManager,
            IGameService gameService,
            MongoDbService mongoDbService)
        {
            _logger = logger;
            _userManager = userManager;
            _gameService = gameService;
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> StartGame(int numberOfPlayers)
        {
            try
            {
                var userId = User?.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
                var game = _gameService.InitializeGame(numberOfPlayers);
                var gamesCollection = _mongoDbService.GetGamesCollection();

                await gamesCollection.InsertOneAsync(game);

                return View("GameBoard", game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartGame error");
                return View("Error");
            }
        }

        public async Task<IActionResult> MovePiece(int pieceId)
        {
            try
            {
                var userId = User?.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;

                var game = await _gameService.MakeMove(userId, moveNotation);
                if (game == null)
                {
                    _logger.LogWarning("There is not active game for user- {UserId}", userId ?? "guest");
                    return RedirectToAction("StartGame");
                }

                return View("GameBoard", game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas wykonywania ruchu");
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}