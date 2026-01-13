using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Intefaces.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly IGameService _gameService;
        private readonly IMongoDbService _mongoDbService;
        private readonly IUserIdentifierService _userIdentifierService;

        public GameController(
            ILogger<GameController> logger,
            IGameService gameService,
            IMongoDbService mongoDbService,
            IUserIdentifierService userIdentifierService)
        {
            _logger = logger;
            _gameService = gameService;
            _mongoDbService = mongoDbService;
            _userIdentifierService = userIdentifierService;
        }

        [HttpGet]
        public async Task<IActionResult> StartGame(int numberOfPlayers = 2)
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();
                var existingGame = await _mongoDbService.GetGamesCollection()
                    .Find(g => g.OwnerId == userId && g.IsGameActive)
                    .FirstOrDefaultAsync();

                if (existingGame != null)
                    return RedirectToAction("LoadLastGame");

                var game = _gameService.InitializeGame(numberOfPlayers);

                await _mongoDbService.GetGamesCollection().InsertOneAsync(game);

                return RedirectToAction("LoadLastGame");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartGame error");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewGame(int numberOfPlayers = 2)
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();
                var games = _mongoDbService.GetGamesCollection();

                await games.UpdateOneAsync(
                    g => g.OwnerId == userId && g.IsGameActive,
                    Builders<Game>.Update.Set(g => g.IsGameActive, false)
                );

                var newGame = _gameService.InitializeGame(numberOfPlayers);

                await games.InsertOneAsync(newGame);

                return RedirectToAction("LoadLastGame");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NewGame error");
                return View("Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectPiece(int pieceId)
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();
                var game = await _gameService.MarkPossibleMoves(userId, pieceId);

                if (game == null)
                    return RedirectToAction("StartGame");

                return RedirectToAction("LoadLastGame");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SelectPiece action error");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MovePieceTo(int x, int y)
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();
                var game = await _gameService.TryMovePieceAsync(userId, x, y);

                if (game == null)
                    return RedirectToAction("StartGame");

                return RedirectToAction("LoadLastGame");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MovePieceTo error");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadLastGame()
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();

                if (userId == null)
                    return RedirectToAction("StartGame");

                var game = await _mongoDbService.GetGamesCollection()
                    .Find(g => g.OwnerId == userId && g.IsGameActive)
                    .FirstOrDefaultAsync();

                if (game == null)
                    return RedirectToAction("StartGame");

                _gameService.MarkPiecesWithLegalMoves(game);
                return View("GameBoard", game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadLastGame error");
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