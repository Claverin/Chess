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
        private readonly IGameTrackerService _gameTrackerService;

        public GameController(
            ILogger<GameController> logger,
            IGameService gameService,
            IMongoDbService mongoDbService,
            IUserIdentifierService userIdentifierService,
            IGameTrackerService gameTrackerService)
        {
            _logger = logger;
            _gameService = gameService;
            _mongoDbService = mongoDbService;
            _userIdentifierService = userIdentifierService;
            _gameTrackerService = gameTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> StartGame(int numberOfPlayers)
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();

                await _mongoDbService.GetGamesCollection().UpdateManyAsync(
                    g => g.OwnerId == userId && g.IsGameActive,
                    Builders<Game>.Update.Set(g => g.IsGameActive, false));

                var game = _gameService.InitializeGame(numberOfPlayers);
                await _mongoDbService.GetGamesCollection().InsertOneAsync(game);

                _gameTrackerService.SetCurrentGameId(game.Id);

                return RedirectToAction("LoadLastGame");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartGame error");
                return View("Error");
            }
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> MovePieceTo(int x, int y)
        {
            var userId = _userIdentifierService.GetUserObjectId();
            var game = await _gameService.TryMovePieceAsync(userId, x, y);

            if (game == null)
                return RedirectToAction("StartGame");

            return RedirectToAction("LoadLastGame");
        }

        [HttpGet]
        public async Task<IActionResult> LoadLastGame()
        {
            var gameId = _gameTrackerService.GetCurrentGameId();
            if (gameId == null) return RedirectToAction("StartGame");

            var userId = _userIdentifierService.GetUserObjectId();

            var game = await _mongoDbService.GetGamesCollection()
                .Find(g => g.Id == gameId && g.Players.Any(p => p.UserId == userId))
                .FirstOrDefaultAsync();

            if (game == null)
                return RedirectToAction("StartGame");

            _gameService.MarkPiecesWithLegalMoves(game);
            return View("GameBoard", game);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}